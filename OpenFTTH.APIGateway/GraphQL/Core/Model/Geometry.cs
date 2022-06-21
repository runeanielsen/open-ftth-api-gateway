using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenFTTH.APIGateway.CoreTypes
{
    public class Geometry
    {
        public Geometry(string geoJsonType, string geoJsonCoordinates)
        {
            GeoJsonType = geoJsonType;
            GeoJsonCoordinates = geoJsonCoordinates;
        }
        public string GeoJsonType { get; set; }

        public string GeoJsonCoordinates { get; set; }


        public static Geometry MapFromNTSGeometry(NetTopologySuite.Geometries.Geometry geometry)
        {
            if (geometry == null)
                return null;

            var writer = new NetTopologySuite.IO.GeoJsonWriter();
            var geometryJson = writer.Write(geometry);

            var geometryObj = JObject.Parse(geometryJson);

            var coordinatesJson = geometryObj["coordinates"].ToString(Formatting.None);
            var typeJson = geometryObj["type"].ToString(Formatting.None).Replace("\"", "");

            return new Geometry(typeJson, coordinatesJson);
        }

        public static Geometry MapToPointFromXY(double x, double y)
        {
            return new Geometry("Point", $"[{x.ToString(System.Globalization.CultureInfo.InvariantCulture)},{y.ToString(System.Globalization.CultureInfo.InvariantCulture)}]");
        }
    }
}
