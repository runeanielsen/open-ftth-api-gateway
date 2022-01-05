using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.RouteNetwork.API.Model;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types
{
    public class RouteNetworkTraceType : ObjectGraphType<NearestRouteNodeTraceResult>
    {
        public RouteNetworkTraceType(ILogger<RouteNetworkTraceType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Id of traced route network node");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name of the traced route network node");
            Field(x => x.Distance, type: typeof(FloatGraphType)).Description("The network distance in meters to the traced route network node.");
            Field(x => x.RouteNetworkSegmentIds, type: typeof(ListGraphType<IdGraphType>)).Description("Route network segment ids of the span segment traversal");
            Field(x => x.RouteNetworkSegmentGeometries, type: typeof(ListGraphType<StringGraphType>)).Description("Route network segment geometries of the span segment traversal");
        }
    }
}
