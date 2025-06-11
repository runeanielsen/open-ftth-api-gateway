using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Events.RouteNetwork.Infos;
using OpenFTTH.Results;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Queries
{
    public class RouteNetworkServiceQueries : ObjectGraphType
    {
        public RouteNetworkServiceQueries(ILogger<RouteNetworkServiceQueries> logger, IQueryDispatcher queryDispatcher)
        {
            Description = "GraphQL API for querying data owned by route nodes and route segments";

            FieldAsync<RouteNetworkElementType>(
                "routeElement",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }),
                resolve: async context =>
                {
                    if (!Guid.TryParse(context.GetArgument<string>("id"), out Guid routeNodeId))
                    {
                        context.Errors.Add(new ExecutionError("Wrong value for guid"));
                        return null;
                    }

                    var routeNodeQuery = new GetRouteNetworkDetails(new RouteNetworkElementIdList() { routeNodeId })
                    {
                        RelatedInterestFilter = RelatedInterestFilterOptions.ReferencesFromRouteElementOnly
                    };

                    var queryResult = await queryDispatcher.HandleAsync<GetRouteNetworkDetails, Result<GetRouteNetworkDetailsResult>>(routeNodeQuery);

                    if (queryResult.IsFailed)
                    {
                        context.Errors.Add(new ExecutionError(queryResult.Errors.First().Message));
                        return null;
                    }

                    return queryResult.Value.RouteNetworkElements[routeNodeId];
                }
            );

            Field<ListGraphType<IdGraphType>>("shortestPathBetweenNodes")
                .Description("Returns the shortest path between two nodes.")
                .Arguments(
                    new QueryArguments(
                        new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "fromNodeId" },
                        new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "toNodeId" }
                    )
                )
                .ResolveAsync(async context =>
                {
                    var fromNodeId = context.GetArgument<Guid>("fromNodeId");
                    var toNodeId = context.GetArgument<Guid>("toNodeId");

                    var shortestPathQuery = new ShortestPathBetweenRouteNodes(fromNodeId, toNodeId);

                    var nearestNodeQueryResult = await queryDispatcher
                        .HandleAsync<ShortestPathBetweenRouteNodes, Result<ShortestPathBetweenRouteNodesResult>>(shortestPathQuery)
                        .ConfigureAwait(false);

                    if (nearestNodeQueryResult.IsFailed)
                    {
                        context.Errors.Add(new ExecutionError(nearestNodeQueryResult.Errors.First().Message));
                        return null;
                    }
                    return nearestNodeQueryResult.Value.RouteNetworkElementIds;
                });

            FieldAsync<ListGraphType<RouteNetworkTraceType>>(
                 "nearestNeighborNodes",
                 arguments: new QueryArguments(
                     new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "sourceRouteNodeId" },
                     new QueryArgument<NonNullGraphType<ListGraphType<RouteNodeKindEnumType>>> { Name = "stops" },
                     new QueryArgument<NonNullGraphType<ListGraphType<RouteNodeKindEnumType>>> { Name = "interests" },
                     new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "maxHits" },
                     new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "maxBirdFlyDistanceMeters" }
                 ),
                 resolve: async context =>
                 {
                     Guid routeNodeId = context.GetArgument<Guid>("sourceRouteNodeId");
                     List<RouteNodeKindEnum> stops = context.GetArgument<List<RouteNodeKindEnum>>("stops");
                     List<RouteNodeKindEnum> interests = context.GetArgument<List<RouteNodeKindEnum>>("interests");
                     int maxHits = context.GetArgument<int>("maxHits");
                     int maxBirdFlyDistanceMeters = context.GetArgument<int>("maxBirdFlyDistanceMeters");


                     var nearestNodeQuery = new FindNearestRouteNodes(routeNodeId, maxHits, maxBirdFlyDistanceMeters, stops.ToArray(), interests.ToArray());

                     var nearestNodeQueryResult = await queryDispatcher.HandleAsync<FindNearestRouteNodes, Result<FindNearestRouteNodesResult>>(nearestNodeQuery);

                     if (nearestNodeQueryResult.IsFailed)
                     {
                         context.Errors.Add(new ExecutionError(nearestNodeQueryResult.Errors.First().Message));
                         return null;
                     }

                     return nearestNodeQueryResult.Value.RouteNetworkTraces;
                 }
            );
        }
    }
}
