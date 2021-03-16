using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using System;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Queries
{
    public class RouteNetworkServiceQueries : ObjectGraphType
    {
        public RouteNetworkServiceQueries(ILogger<RouteNetworkServiceQueries> logger, IQueryDispatcher queryDispatcher)
        {
            Description = "GraphQL API for querying data owned by route nodes and route segments";

            Field<RouteNetworkElementType>(
                "routeElement",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }),
                resolve: context =>
                {
                    if (!Guid.TryParse(context.GetArgument<string>("id"), out Guid routeNodeId))
                    {
                        context.Errors.Add(new ExecutionError("Wrong value for guid"));
                        return null;
                    }

                    var routeNodeQuery = new GetRouteNetworkDetails(new RouteNetworkElementIdList() { routeNodeId });

                    var queryResult = queryDispatcher.HandleAsync<GetRouteNetworkDetails, Result<GetRouteNetworkDetailsResult>>(routeNodeQuery).Result;

                    if (queryResult.IsFailed)
                    {
                        context.Errors.Add(new ExecutionError(queryResult.Errors.First().Message));
                        return null;
                    }

                    return queryResult.Value.RouteNetworkElements[routeNodeId];
                }
           );

        }

    }
}
