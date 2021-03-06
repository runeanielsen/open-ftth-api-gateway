﻿using FluentResults;
using GraphQL;
using GraphQL.Types;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.CQRS;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.EventSourcing;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.UtilityGraphService.API.Commands;
using System;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Mutations
{
    public class NodeContainerMutations : ObjectGraphType
    {
        public NodeContainerMutations(ICommandDispatcher commandDispatcher, IEventStore eventStore)
        {
            Description = "Node container mutations";

            Field<CommandResultType>(
              "placeNodeContainerInRouteNetwork",
              description: "Place a node container (i.e. conduit closure, well, cabinet whatwever) in a route network node",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerSpecificationId" },
                  new QueryArgument<IdGraphType> { Name = "manufacturerId" }
              ),
              resolve: context =>
              {
                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  Guid correlationId = Guid.NewGuid();

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");

                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");
                  var nodeContainerSpecificationId = context.GetArgument<Guid>("nodeContainerSpecificationId");
                  var manufacturerId = context.GetArgument<Guid>("manufacturerId");

                  // First register the walk in the route network where the client want to place the node container
                  var nodeOfInterestId = Guid.NewGuid();
                  var walk = new RouteNetworkElementIdList();
                  var registerNodeOfInterestCommand = new RegisterNodeOfInterest(nodeOfInterestId, routeNodeId)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var registerNodeOfInterestCommandResult = commandDispatcher.HandleAsync<RegisterNodeOfInterest, Result<RouteNetworkInterest>>(registerNodeOfInterestCommand).Result;

                  if (registerNodeOfInterestCommandResult.IsFailed)
                  {
                      return new CommandResult(registerNodeOfInterestCommandResult);
                  }

                  // Now place the conduit in the walk
                  var placeNodeContainerCommand = new PlaceNodeContainerInRouteNetwork(nodeContainerId, nodeContainerSpecificationId, registerNodeOfInterestCommandResult.Value)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId),
                      ManufacturerId = manufacturerId,
                      LifecycleInfo = new LifecycleInfo(DeploymentStateEnum.InService, null, null)
                  };

                  var placeNodeContainerResult = commandDispatcher.HandleAsync<PlaceNodeContainerInRouteNetwork, Result>(placeNodeContainerCommand).Result;

                  // Unregister interest if place node container failed
                  if (placeNodeContainerResult.IsFailed)
                  {
                      var unregisterCommandResult = commandDispatcher.HandleAsync<UnregisterInterest, Result>(new UnregisterInterest(nodeOfInterestId)).Result;

                      if (unregisterCommandResult.IsFailed)
                          return new CommandResult(unregisterCommandResult);
                  }

                  return new CommandResult(placeNodeContainerResult);
              }
            );

            Field<CommandResultType>(
                 "reverseVerticalContentAlignment",
                 description: "Toggle whether the content in the node container should be drawed from bottom up or top down",
                 arguments: new QueryArguments(
                     new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerId" }
                 ),
                 resolve: context =>
                 {
                     var userContext = context.UserContext as GraphQLUserContext;
                     var userName = userContext.Username;

                     Guid correlationId = Guid.NewGuid();

                     // TODO: Get from work manager
                     var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                     var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");

                     var reverseAlignmentCmd = new ReverseNodeContainerVerticalContentAlignment(nodeContainerId)
                     {
                         CorrelationId = correlationId,
                         UserContext = new UserContext(userName, workTaskId)
                     };

                     var reverseAlignmentCmdResult = commandDispatcher.HandleAsync<ReverseNodeContainerVerticalContentAlignment, Result>(reverseAlignmentCmd).Result;

                     return new CommandResult(reverseAlignmentCmdResult);
                 }
           );
        }
    }
}
