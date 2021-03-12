using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanSegmentTraceType : ObjectGraphType<SpanSegmentTrace>
    {
        public SpanSegmentTraceType(ILogger<SpanSegmentTraceType> logger)
        {
            Field(x => x.RouteNetworkSegmentIds, type: typeof(ListGraphType<IdGraphType>)).Description("Route network segments that this span segment is connected to");
        }
    }
}
