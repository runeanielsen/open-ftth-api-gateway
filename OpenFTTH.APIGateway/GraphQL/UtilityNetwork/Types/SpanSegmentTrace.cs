using System;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanSegmentTrace
    {
        public Guid[] RouteNetworkSegmentIds { get; set; }
        public string[] RouteNetworkSegmentGeometries { get; set; }
        public Guid[] RouteNetworkNodeIds { get; set; }
        public double WGS84MinX { get; set; }
        public double WGS84MinY { get; set; }
        public double WGS84MaxX { get; set; }
        public double WGS84MaxY { get; set; }
        public double ETRS89MinX { get; set; }
        public double ETRS89MinY { get; set; }
        public double ETRS89MaxX { get; set; }
        public double ETRS89MaxY { get; set; }
    }
}
