using GraphQL.Types;
using Microsoft.Extensions.Logging;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanSegmentTraceType : ObjectGraphType<SpanSegmentTrace>
    {
        public SpanSegmentTraceType(ILogger<SpanSegmentTraceType> logger)
        {
            Field(x => x.RouteNetworkSegmentIds, type: typeof(ListGraphType<IdGraphType>)).Description("Route network segment ids of the span segment traversal");

            Field(x => x.RouteNetworkSegmentGeometries, type: typeof(ListGraphType<StringGraphType>)).Description("Route network segment geometries of the span segment traversal");

            Field(x => x.WGS84MinX, type: typeof(FloatGraphType)).Description("MinX in WGS89");
            Field(x => x.WGS84MinY, type: typeof(FloatGraphType)).Description("MinY in WGS89");
            Field(x => x.WGS84MaxX, type: typeof(FloatGraphType)).Description("MaxX in WGS89");
            Field(x => x.WGS84MaxY, type: typeof(FloatGraphType)).Description("MaxY in WGS89");

            Field(x => x.ETRS89MinX, type: typeof(FloatGraphType)).Description("MinX in ETRS89");
            Field(x => x.ETRS89MinY, type: typeof(FloatGraphType)).Description("MinY in ETRS89");
            Field(x => x.ETRS89MaxX, type: typeof(FloatGraphType)).Description("MaxX in ETRS89");
            Field(x => x.ETRS89MaxY, type: typeof(FloatGraphType)).Description("MaxY in ETRS89");
        }
    }
}
