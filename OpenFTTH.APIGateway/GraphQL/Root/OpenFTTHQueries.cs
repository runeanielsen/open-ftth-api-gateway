using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Queries;
using OpenFTTH.APIGateway.GraphQL.Work.Queries;

namespace OpenFTTH.APIGateway.GraphQL.Root
{
    public class OpenFTTHQueries : ObjectGraphType
    {
        public OpenFTTHQueries(ILogger<OpenFTTHQueries> logger)
        {
            Description = "GraphQL API for querying Open FTTH";

            Field<StringGraphType>("apiVersion", resolve: context => VersionInfo.VersionString());

            Field<RouteNetworkServiceQueries>("routeNetworkService", resolve: context => new { });

            Field<WorkServiceQueries>("workService", resolve: context => new { });
        }
    }
}
