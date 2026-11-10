using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Buffer;
using OpenFTTH.Schematic.API.Model.DiagramLayout;
using OpenFTTH.Schematic.Business.Drawing;
using System;
using System.Collections.Generic;

namespace OpenFTTH.Schematic.Business.Lines
{
    public class LineBlockTerminalConnection
    {
        // Half width of connections drawn as polygons (conduits)
        private const double PolygonHalfWidth = 4;

        // Number of segments used to draw the arc in rounded corners
        private const int ArcSegments = 24;

        public LineShapeTypeEnum LineShapeType { get; set; }
        public BlockPortTerminal FromTerminal { get; set; }
        public BlockPortTerminal ToTerminal { get; set; }

        public string Label { get; set; }
        public string Style { get; set; }
        public int DrawingOrder { get; set; }

        public List<KeyValuePair<string, string>> Properties { get; set; }


        private Guid _refId;
        private string _refClass;
        private Envelope _canvasEnvelope = null;


        public void SetReference(Guid refId, string refClass)
        {
            this._refId = refId;
            this._refClass = refClass;
        }

        internal IEnumerable<DiagramObject> CreateDiagramObjects(Diagram diagram)
        {
            _canvasEnvelope = diagram.Envelope;

            var curve = CreateCurve();

            Geometry geometry = curve;

            if (LineShapeType == LineShapeTypeEnum.Polygon)
            {
                // Buffer the center line to get a band of constant width, also through curves and loops
                var bufferParameters = new BufferParameters() { EndCapStyle = NetTopologySuite.Operation.Buffer.EndCapStyle.Flat };
                geometry = curve.Buffer(GeometryBuilder.Convert(PolygonHalfWidth), bufferParameters);
            }

            return new List<DiagramObject>
            {
                new DiagramObject(diagram)
                {
                    IdentifiedObject = _refClass == null ? null : new IdentifiedObjectReference() { RefId = _refId, RefClass = _refClass },
                    Style = Style is null ? "Cable" : Style,
                    Label = this.Label,
                    Geometry = geometry,
                    DrawingOrder = DrawingOrder,
                    Properties = Properties
                }
            };
        }

        private LineString CreateCurve()
        {
            var fromSide = FromTerminal.Port.Side;
            var toSide = ToTerminal.Port.Side;

            double fromX = FromTerminal.ConnectionPointX;
            double fromY = FromTerminal.ConnectionPointY;
            double toX = ToTerminal.ConnectionPointX;
            double toY = ToTerminal.ConnectionPointY;

            // The curve starts/ends where the terminal stub ends inside the block
            var (fromDirX, fromDirY) = InwardDirection(fromSide);
            var (toDirX, toDirY) = InwardDirection(toSide);

            double fromCurveX = fromX + fromDirX * FromTerminal.Thickness;
            double fromCurveY = fromY + fromDirY * FromTerminal.Thickness;
            double toCurveX = toX + toDirX * ToTerminal.Thickness;
            double toCurveY = toY + toDirY * ToTerminal.Thickness;

            bool fromHorizontal = IsHorizontalSide(fromSide);
            bool toHorizontal = IsHorizontalSide(toSide);

            // Same side - loop into the block and back
            if (fromSide == toSide)
            {
                double span = fromHorizontal ? Math.Abs(fromCurveY - toCurveY) : Math.Abs(fromCurveX - toCurveX);

                // Prevent that the loop is drawn outside the canvas
                double canvasSize = (fromHorizontal ? _canvasEnvelope.Width : _canvasEnvelope.Height) * 10000;
                if (canvasSize < span)
                    span = canvasSize * 0.9;

                double bulgeX = fromDirX * span;
                double bulgeY = fromDirY * span;

                return GeometryBuilder.Beizer(
                    fromCurveX, fromCurveY,
                    fromCurveX + bulgeX, fromCurveY + bulgeY,
                    toCurveX + bulgeX, toCurveY + bulgeY,
                    toCurveX, toCurveY,
                    fromX, fromY, toX, toY);
            }

            // Terminals aligned - draw straight line
            if (fromX == toX || fromY == toY)
                return GeometryBuilder.StraightLine(fromX, fromY, toX, toY);

            // Perpendicular sides - straight legs joined by a circular arc in the corner
            if (fromHorizontal != toHorizontal)
                return CreateRoundedCorner(fromX, fromY, fromCurveX, fromCurveY, toCurveX, toCurveY, toX, toY, fromHorizontal);

            // Opposite sides - S-curve leaving and entering the terminals in their own direction
            double halfX = fromHorizontal ? (toCurveX - fromCurveX) / 2 : 0;
            double halfY = fromHorizontal ? 0 : (toCurveY - fromCurveY) / 2;

            return GeometryBuilder.Beizer(
                fromCurveX, fromCurveY,
                fromCurveX + halfX, fromCurveY + halfY,
                toCurveX - halfX, toCurveY - halfY,
                toCurveX, toCurveY,
                fromX, fromY, toX, toY);
        }

        /// <summary>
        /// Straight legs meeting in a corner, rounded with a quarter circle as large as the shortest leg allows.
        /// Connections in a bundle get concentric arcs, so the spacing between them stays the same through the corner.
        /// </summary>
        private static LineString CreateRoundedCorner(double fromX, double fromY, double fromCurveX, double fromCurveY, double toCurveX, double toCurveY, double toX, double toY, bool fromHorizontal)
        {
            double cornerX = fromHorizontal ? toCurveX : fromCurveX;
            double cornerY = fromHorizontal ? fromCurveY : toCurveY;

            // Leg lengths and directions from the corner back towards each terminal
            double fromLegLength = Math.Abs(fromCurveX - cornerX) + Math.Abs(fromCurveY - cornerY);
            double toLegLength = Math.Abs(toCurveX - cornerX) + Math.Abs(toCurveY - cornerY);

            double radius = Math.Min(fromLegLength, toLegLength);

            double arcStartX = cornerX + Math.Sign(fromCurveX - cornerX) * radius;
            double arcStartY = cornerY + Math.Sign(fromCurveY - cornerY) * radius;
            double arcEndX = cornerX + Math.Sign(toCurveX - cornerX) * radius;
            double arcEndY = cornerY + Math.Sign(toCurveY - cornerY) * radius;

            double centerX = arcStartX + arcEndX - cornerX;
            double centerY = arcStartY + arcEndY - cornerY;

            double startAngle = Math.Atan2(arcStartY - centerY, arcStartX - centerX);
            double sweep = Math.Atan2(arcEndY - centerY, arcEndX - centerX) - startAngle;

            if (sweep > Math.PI)
                sweep -= 2 * Math.PI;
            else if (sweep < -Math.PI)
                sweep += 2 * Math.PI;

            var pnts = new List<Coordinate>();

            void AddPoint(double x, double y)
            {
                var coord = new Coordinate(GeometryBuilder.Convert(x), GeometryBuilder.Convert(y));

                if (pnts.Count == 0 || !pnts[pnts.Count - 1].Equals2D(coord))
                    pnts.Add(coord);
            }

            AddPoint(fromX, fromY);
            AddPoint(fromCurveX, fromCurveY);

            for (int i = 0; i <= ArcSegments; i++)
            {
                double angle = startAngle + sweep * i / ArcSegments;
                AddPoint(centerX + radius * Math.Cos(angle), centerY + radius * Math.Sin(angle));
            }

            AddPoint(toCurveX, toCurveY);
            AddPoint(toX, toY);

            return new LineString(pnts.ToArray());
        }

        /// <summary>
        /// West and east terminals point horizontally into the block, north and south vertically
        /// </summary>
        private static bool IsHorizontalSide(BlockSideEnum side)
        {
            return side == BlockSideEnum.West || side == BlockSideEnum.East;
        }

        /// <summary>
        /// Unit vector pointing from the block side into the block
        /// </summary>
        private static (double X, double Y) InwardDirection(BlockSideEnum side)
        {
            return side switch
            {
                BlockSideEnum.West => (1, 0),
                BlockSideEnum.East => (-1, 0),
                BlockSideEnum.North => (0, -1),
                BlockSideEnum.South => (0, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }
    }
}
