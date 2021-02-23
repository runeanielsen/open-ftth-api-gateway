using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.APIGateway.Remote;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test;
using System;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Queries
{
    public class RouteNetworkServiceQueries : ObjectGraphType
    {
        public RouteNetworkServiceQueries(ILogger<RouteNetworkServiceQueries> logger)
        {
            Description = "GraphQL API for querying data owned by route nodes and route segments";

            Field<RouteNodeType>(
                "routeNode",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }),
                resolve: context =>
                {
                    Guid id;
                    if (!Guid.TryParse(context.GetArgument<string>("id"), out id))
                    {
                        context.Errors.Add(new ExecutionError("Wrong value for guid"));
                        return null;
                    }

                    logger.LogDebug("Route node query: " + id);

                    // For quick testing... should be removed
                    return RouteNetworkFakeState.GetRouteNodeState(id);

                    // Call the route network service
                    //return routeNetworkQueries.Query<RouteNodeQuery, RouteNodeQueryResult>(new RouteNodeQuery(id));
                }
            );


            Field<RouteSegmentType>(
               "routeSegment",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }),
               resolve: context =>
               {
                   Guid id;
                   if (!Guid.TryParse(context.GetArgument<string>("id"), out id))
                   {
                       context.Errors.Add(new ExecutionError("Wrong value for guid"));
                       return null;
                   }

                   logger.LogDebug("Route segment query: " + id);

                  // For quick testing... should be removed
                  return RouteNetworkFakeState.GetRouteSegmentState(id);
               }
           );

        }

    }
}
