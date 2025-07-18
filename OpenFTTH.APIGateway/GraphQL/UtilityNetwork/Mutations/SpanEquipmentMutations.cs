﻿using OpenFTTH.Results;
using GraphQL;
using GraphQL.Types;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Addresses.Types;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.Graph;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments.Projections;
using System;
using System.Collections.Generic;
using OpenFTTH.APIGateway.GraphQL.Work;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Mutations
{
    public class SpanEquipmentMutations : ObjectGraphType
    {
        public SpanEquipmentMutations(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IEventStore eventStore)
        {
            Description = "Span equipment mutations";

            Field<CommandResultType>("placeSpanEquipmentInRouteNetwork")
              .Description("Place a span equipment (i.e. conduit, cable whatwever) in the route network")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentSpecificationId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "routeSegmentIds" },
                  new QueryArgument<IdGraphType> { Name = "manufacturerId" },
                  new QueryArgument<MarkingInfoInputType> { Name = "markingInfo" },
                  new QueryArgument<NamingInfoInputType> { Name = "namingInfo" },
                  new QueryArgument<AddressInfoInputType> { Name = "addressInfo" }
              ))
              .ResolveAsync(async context =>
              {
                  var spanEquipmentId = context.GetArgument<Guid>("spanEquipmentId");
                  var spanEquipmentSpecificationId = context.GetArgument<Guid>("spanEquipmentSpecificationId");
                  var routeSegmentIds = context.GetArgument<List<Guid>>("routeSegmentIds");
                  var manufacturerId = context.GetArgument<Guid>("manufacturerId");
                  var markingInfo = context.GetArgument<MarkingInfo>("markingInfo");
                  var namingInfo = context.GetArgument<NamingInfo>("namingInfo");
                  var addressInfo = context.GetArgument<AddressInfo>("addressInfo");

                  // Name conduit span equipment
                  // TODO: Refactor into som class responsible for span equipment naming
                  var spec = eventStore.Projections.Get<SpanEquipmentSpecificationsProjection>().Specifications[spanEquipmentSpecificationId];
                  namingInfo = CalculateSpanEquipmentName(eventStore, namingInfo, spec);

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                  var spanEquipments = eventStore.Projections.Get<UtilityNetworkProjection>().SpanEquipmentsByEquipmentId;

                  // First register the walk in the route network where the client want to place the span equipment
                  var walkOfInterestId = Guid.NewGuid();
                  var walk = new RouteNetworkElementIdList();
                  walk.AddRange(routeSegmentIds);

                  var registerWalkOfInterestCommand = new RegisterWalkOfInterest(correlationId, commandUserContext, walkOfInterestId, walk);

                  var registerWalkOfInterestCommandResult = await commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

                  if (registerWalkOfInterestCommandResult.IsFailed)
                  {
                      return new CommandResult(registerWalkOfInterestCommandResult);
                  }

                  // Now place the conduit in the walk
                  var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(
                      correlationId, commandUserContext, spanEquipmentId, spanEquipmentSpecificationId, registerWalkOfInterestCommandResult.Value)
                  {
                      ManufacturerId = manufacturerId,
                      NamingInfo = namingInfo,
                      MarkingInfo = markingInfo,
                      LifecycleInfo = new LifecycleInfo(DeploymentStateEnum.InService, null, null),
                      AddressInfo = addressInfo
                  };

                  var placeSpanEquipmentResult = await commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

                  // Unregister interest if place span equipment failed
                  if (placeSpanEquipmentResult.IsFailed)
                  {
                      var unregisterCommandResult = await commandDispatcher.HandleAsync<UnregisterInterest, Result>(
                          new UnregisterInterest(correlationId, commandUserContext, walkOfInterestId));

                      if (unregisterCommandResult.IsFailed)
                          return new CommandResult(unregisterCommandResult);
                  }

                  return new CommandResult(placeSpanEquipmentResult);
              });

            Field<CommandResultType>("affixSpanEquipmentToNodeContainer")
              .Description("Affix a span equipment to a node container - i.e. to some condult closure, man hole etc.")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentIds" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerId" },
                  new QueryArgument<NonNullGraphType<NodeContainerSideEnumType>> { Name = "nodeContainerSide" }
              ))
              .ResolveAsync(async context =>
              {
                  var spanSegmentIds = context.GetArgument<List<Guid>>("spanSegmentIds");
                  var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");
                  var side = context.GetArgument<NodeContainerSideEnum>("nodeContainerSide");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                  foreach (var spanSegmentId in spanSegmentIds)
                  {
                      var affixCommand = new AffixSpanEquipmentToNodeContainer(correlationId, commandUserContext, spanSegmentId, nodeContainerId, side);

                      var affixCommandResult = await commandDispatcher.HandleAsync<AffixSpanEquipmentToNodeContainer, Result>(affixCommand);

                      if (affixCommandResult.IsFailed)
                          return new CommandResult(affixCommandResult);
                  }

                  return new CommandResult(Result.Ok());
              });

            Field<CommandResultType>("detachSpanEquipmentFromNodeContainer")
              .Description("Detach a span equipment from a node container - i.e. from some condult closure, man hole etc.")
              .Arguments(new QueryArguments(
                  new QueryArgument<ListGraphType<IdGraphType>> { Name = "spanSegmentIds" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" }
              ))
              .ResolveAsync(async context =>
              {
                  var spanSegmentIds = context.GetArgument<List<Guid>>("spanSegmentIds");
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value)
                  {
                      EditingRouteNodeId = routeNodeId
                  };

                  foreach (var spanSegmentId in spanSegmentIds)
                  {
                      var detachCommand = new DetachSpanEquipmentFromNodeContainer(correlationId, commandUserContext, spanSegmentId, routeNodeId);
                      var detachCommandResult = await commandDispatcher.HandleAsync<DetachSpanEquipmentFromNodeContainer, Result>(detachCommand);

                      if (detachCommandResult.IsFailed)
                          return new CommandResult(detachCommandResult);
                  }

                  return new CommandResult(Result.Ok());
              });

            Field<CommandResultType>("cutSpanSegments")
              .Description("Cut the span segments belonging to som span equipment at the route node specified")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentstoCut" }
              ))
              .ResolveAsync(async context =>
              {
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var spanSegmentToCut = context.GetArgument<Guid[]>("spanSegmentstoCut");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value)
                  {
                      EditingRouteNodeId = routeNodeId
                  };

                  var cutCmd = new CutSpanSegmentsAtRouteNode(correlationId, commandUserContext, routeNodeId, spanSegmentToCut);
                  var cutResult = await commandDispatcher.HandleAsync<CutSpanSegmentsAtRouteNode, Result>(cutCmd);

                  return new CommandResult(cutResult);
              });

            Field<CommandResultType>("connectSpanSegments")
              .Description("Connect the span segments belonging to two different span equipment at the route node specified")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentsToConnect" }
              ))
              .ResolveAsync(async context =>
              {
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var spanSegmentToConnect = context.GetArgument<Guid[]>("spanSegmentsToConnect");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value)
                  {
                      EditingRouteNodeId = routeNodeId
                  };

                  var connectCmd = new ConnectSpanSegmentsAtRouteNode(correlationId, commandUserContext, routeNodeId, spanSegmentToConnect);
                  var connectResult = await commandDispatcher.HandleAsync<ConnectSpanSegmentsAtRouteNode, Result>(connectCmd);

                  return new CommandResult(connectResult);
              });

            Field<CommandResultType>("disconnectSpanSegments")
              .Description("Disconnect two span segments belonging to two different span equipment at the route node specified")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentsToDisconnect" }
              ))
              .ResolveAsync(async context =>
              {
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var spanSegmentToDisconnect = context.GetArgument<Guid[]>("spanSegmentsToDisconnect");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value)
                  {
                      EditingRouteNodeId = routeNodeId
                  };

                  var disconnectCmd = new DisconnectSpanSegmentsAtRouteNode(correlationId, commandUserContext, routeNodeId, spanSegmentToDisconnect);
                  var disconnectResult = await commandDispatcher.HandleAsync<DisconnectSpanSegmentsAtRouteNode, Result>(disconnectCmd);

                  return new CommandResult(disconnectResult);
              });

            Field<CommandResultType>("addAdditionalInnerSpanStructures")
              .Description("Add inner span structures to an existing span equipment")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanStructureSpecificationIds" }
              ))
              .ResolveAsync(async context =>
              {
                  var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");
                  var specificationsId = context.GetArgument<Guid[]>("spanStructureSpecificationIds");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                  var addStructure = new PlaceAdditionalStructuresInSpanEquipment(
                    correlationId: correlationId,
                    userContext: commandUserContext,
                    spanEquipmentId: spanEquipmentOrSegmentId,
                    structureSpecificationIds: specificationsId
                  );

                  var addStructureResult = await commandDispatcher.HandleAsync<PlaceAdditionalStructuresInSpanEquipment, Result>(addStructure);

                  return new CommandResult(addStructureResult);
              });

            Field<CommandResultType>("removeSpanStructure")
              .Description("Remove inner or outer span structure of a span equipment. When the outer span structure is removed the entire span equipment is removed from the network.")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId" }
              ))
              .ResolveAsync(async context =>
              {
                  var spanSegmentId = context.GetArgument<Guid>("spanSegmentId");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                  var removeStructure = new RemoveSpanStructureFromSpanEquipment(
                    correlationId: correlationId,
                    userContext: commandUserContext,
                    spanSegmentId: spanSegmentId
                  );

                  var removeStructureResult = await commandDispatcher.HandleAsync<RemoveSpanStructureFromSpanEquipment, Result>(removeStructure);

                  return new CommandResult(removeStructureResult);
              });

            Field<CommandResultType>("move")
              .Description("Move a span equipment / change its walk in the route network")
              .Arguments(new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "routeSegmentIds" }
              ))
              .ResolveAsync(async context =>
              {
                  var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // Get the users current work task (will fail, if user has not selected a work task)
                  var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                  if (currentWorkTaskIdResult.IsFailed)
                      return new CommandResult(currentWorkTaskIdResult);

                  var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                  Guid[] routeSegmentIds = context.GetArgument<Guid[]>("routeSegmentIds");

                  RouteNetworkElementIdList newWalkIds = new();
                  newWalkIds.AddRange(routeSegmentIds);

                  var moveCmd = new MoveSpanEquipment(
                    correlationId: correlationId,
                    userContext: commandUserContext,
                    spanEquipmentId: spanEquipmentOrSegmentId,
                    newWalkIds: newWalkIds
                  );

                  var moveCmdResult = await commandDispatcher.HandleAsync<MoveSpanEquipment, Result>(moveCmd);

                  return new CommandResult(moveCmdResult);
              });

            Field<CommandResultType>("updateProperties")
             .Description("Mutation that can be used to change the span equipment specification, manufacturer and/or marking information")
             .Arguments(new QueryArguments(
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" },
                 new QueryArgument<IdGraphType> { Name = "spanEquipmentSpecificationId" },
                 new QueryArgument<IdGraphType> { Name = "manufacturerId" },
                 new QueryArgument<NamingInfoInputType> { Name = "namingInfo" },
                 new QueryArgument<MarkingInfoInputType> { Name = "markingInfo" },
                 new QueryArgument<AddressInfoInputType> { Name = "addressInfo" }
             ))
             .ResolveAsync(async context =>
             {
                 var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");

                 var correlationId = Guid.NewGuid();

                 var userContext = context.UserContext as GraphQLUserContext;
                 var userName = userContext.Username;

                 // Get the users current work task (will fail, if user has not selected a work task)
                 var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                 if (currentWorkTaskIdResult.IsFailed)
                     return new CommandResult(currentWorkTaskIdResult);

                 var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                 var updateCmd = new UpdateSpanEquipmentProperties(correlationId, commandUserContext, spanEquipmentOrSegmentId: spanEquipmentOrSegmentId)
                 {
                     SpecificationId = context.HasArgument("spanEquipmentSpecificationId") ? context.GetArgument<Guid>("spanEquipmentSpecificationId") : null,
                     ManufacturerId = context.HasArgument("manufacturerId") ? context.GetArgument<Guid>("manufacturerId") : null,
                     NamingInfo = context.HasArgument("namingInfo") ? context.GetArgument<NamingInfo>("namingInfo") : null,
                     MarkingInfo = context.HasArgument("markingInfo") ? context.GetArgument<MarkingInfo>("markingInfo") : null,
                     AddressInfo = context.HasArgument("addressInfo") ? context.GetArgument<AddressInfo>("addressInfo") : null
                 };

                 var updateResult = await commandDispatcher.HandleAsync<UpdateSpanEquipmentProperties, Result>(updateCmd);

                 return new CommandResult(updateResult);
             });

            Field<CommandResultType>("affixSpanEquipmentToParent")
             .Description("Affix a span equipment to a parent span equipment - i.e. put a cable inside a conduit")
             .Arguments(new QueryArguments(
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId1" },
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId2" }
             ))
             .ResolveAsync(async context =>
             {
                 var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                 var spanSegmentId1 = context.GetArgument<Guid>("spanSegmentId1");
                 var spanSegmentId2 = context.GetArgument<Guid>("spanSegmentId2");

                 var correlationId = Guid.NewGuid();

                 var userContext = context.UserContext as GraphQLUserContext;
                 var userName = userContext.Username;

                 // Get the users current work task (will fail, if user has not selected a work task)
                 var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                 if (currentWorkTaskIdResult.IsFailed)
                     return new CommandResult(currentWorkTaskIdResult);

                 var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                 var affixCommand = new AffixSpanEquipmentToParent(correlationId, commandUserContext, routeNodeId, spanSegmentId1, spanSegmentId2);
                 var affixCommandResult = await commandDispatcher.HandleAsync<AffixSpanEquipmentToParent, Result>(affixCommand);

                 if (affixCommandResult.IsFailed)
                     return new CommandResult(affixCommandResult);

                 return new CommandResult(Result.Ok());
             });

            Field<CommandResultType>("connectToTerminalEquipment")
                .Description("Connect one or more span segments inside a span equipment to terminals inside a terminal equipmment")
                .Arguments(new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                    new QueryArgument<NonNullGraphType<ListGraphType<ConnectSpanSegmentToTerminalOperationInputType>>> { Name = "connects" }
                ))
                .ResolveAsync(async context =>
                {
                    var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                    var connects = context.GetArgument<ConnectSpanSegmentToTerminalOperation[]>("connects");

                    var correlationId = Guid.NewGuid();

                    var userContext = context.UserContext as GraphQLUserContext;
                    var userName = userContext.Username;

                    // Get the users current work task (will fail, if user has not selected a work task)
                    var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                    if (currentWorkTaskIdResult.IsFailed)
                        return new CommandResult(currentWorkTaskIdResult);

                    var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);

                    var connectCommand = new ConnectSpanSegmentsWithTerminalsAtRouteNode(correlationId, commandUserContext, routeNodeId, connects);
                    var connectCommandResult = await commandDispatcher.HandleAsync<ConnectSpanSegmentsWithTerminalsAtRouteNode, Result>(connectCommand);

                    if (connectCommandResult.IsFailed)
                        return new CommandResult(connectCommandResult);

                    return new CommandResult(Result.Ok());
                });

            Field<CommandResultType>("disconnectFromTerminalEquipment")
               .Description("Disconnect one or more span segments inside a span equipment from terminals inside a terminal equipmment")
               .Arguments(new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                   new QueryArgument<NonNullGraphType<ListGraphType<DisconnectSpanSegmentFromTerminalOperationInputType>>> { Name = "disconnects" }
               ))
               .ResolveAsync(async context =>
               {
                   var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                   var connects = context.GetArgument<DisconnectSpanSegmentFromTerminalOperation[]>("disconnects");

                   var correlationId = Guid.NewGuid();

                   var userContext = context.UserContext as GraphQLUserContext;
                   var userName = userContext.Username;

                   // Get the users current work task (will fail, if user has not selected a work task)
                   var currentWorkTaskIdResult = WorkQueryHelper.GetUserCurrentWorkId(userName, queryDispatcher);

                   if (currentWorkTaskIdResult.IsFailed)
                       return new CommandResult(currentWorkTaskIdResult);

                   var commandUserContext = new UserContext(userName, currentWorkTaskIdResult.Value);
                   var connectCommand = new DisconnectSpanSegmentsFromTerminalsAtRouteNode(correlationId, commandUserContext, routeNodeId, connects);

                   var connectCommandResult = await commandDispatcher.HandleAsync<DisconnectSpanSegmentsFromTerminalsAtRouteNode, Result>(connectCommand);

                   if (connectCommandResult.IsFailed)
                       return new CommandResult(connectCommandResult);

                   return new CommandResult(Result.Ok());
               });
        }

        private static NamingInfo CalculateSpanEquipmentName(IEventStore eventStore, NamingInfo namingInfo, SpanEquipmentSpecification spec)
        {
            if (spec.IsCable)
            {
                var nextCableSeqStr = eventStore.Sequences.GetNextVal("cable").ToString();

                var conduitName = "K" + nextCableSeqStr.PadLeft(6, '0');
                namingInfo = new NamingInfo(conduitName, null);
            }
            else
            {
                var nextConduitSeqStr = eventStore.Sequences.GetNextVal("conduit").ToString();

                var conduitName = "R" + nextConduitSeqStr.PadLeft(6, '0');
                namingInfo = new NamingInfo(conduitName, null);
            }

            return namingInfo;
        }
    }
}
