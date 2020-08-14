using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Queries
{
    public class OpenFTTHQueries : ObjectGraphType
    {
        public OpenFTTHQueries(ILogger<OpenFTTHQueries> logger)
        {
            Description = "GraphQL API for querying Open FTTH";

            Field<StringGraphType>("apiVersion", resolve: context => VersionInfo.VersionString());

            Field<RouteNetworkServiceQueries>("routeNetwork", resolve: context => new { });
        }
    }
}
