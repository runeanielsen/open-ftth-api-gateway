﻿using FluentResults;
using GraphQL;
using GraphQL.Types;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.EventSourcing;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.Graph;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments.Projections;
using System;
using System.Collections.Generic;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Mutations
{
    public class SpanEquipmentMutations : ObjectGraphType
    {
        public SpanEquipmentMutations(ICommandDispatcher commandDispatcher, IEventStore eventStore)
        {
            Description = "Span equipment mutations";

            Field<CommandResultType>(
              "placeSpanEquipmentInRouteNetwork",
              description: "Place a span equipment (i.e. conduit, cable whatwever) in the route network",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentSpecificationId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "routeSegmentIds" },
                  new QueryArgument<IdGraphType> { Name = "manufacturerId" },
                  new QueryArgument<MarkingInfoInputType> { Name = "markingInfo" },
                  new QueryArgument<NamingInfoInputType> { Name = "namingInfo" }
              ),
              resolve: context =>
              {
                  var spanEquipmentId = context.GetArgument<Guid>("spanEquipmentId");
                  var spanEquipmentSpecificationId = context.GetArgument<Guid>("spanEquipmentSpecificationId");
                  var routeSegmentIds = context.GetArgument<List<Guid>>("routeSegmentIds");
                  var manufacturerId = context.GetArgument<Guid>("manufacturerId");
                  var markingInfo = context.GetArgument<MarkingInfo>("markingInfo");
                  var namingInfo = context.GetArgument<NamingInfo>("namingInfo");

                  // Name conduit span equipment
                  // TODO: Refactor into som class responsible for span equipment naming
                  var spec = eventStore.Projections.Get<SpanEquipmentSpecificationsProjection>().Specifications[spanEquipmentSpecificationId];
                  if (spec.Category != null && spec.Category.ToLower().Contains("conduit"))
                  {
                      var nextConduitSeqStr = eventStore.Sequences.GetNextVal("conduit").ToString();

                      var conduitName = "R" + nextConduitSeqStr.PadLeft(6, '0');
                      namingInfo = new NamingInfo(conduitName, null);
                  }

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;
             

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");

                  var spanEquipments = eventStore.Projections.Get<UtilityNetworkProjection>().SpanEquipments;

                  // First register the walk in the route network where the client want to place the span equipment
                  var walkOfInterestId = Guid.NewGuid();
                  var walk = new RouteNetworkElementIdList();
                  walk.AddRange(routeSegmentIds);

                  var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walk)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var registerWalkOfInterestCommandResult = commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand).Result;

                  if (registerWalkOfInterestCommandResult.IsFailed)
                  {
                      return new CommandResult(registerWalkOfInterestCommandResult);
                  }

                  // Now place the conduit in the walk
                  var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(spanEquipmentId, spanEquipmentSpecificationId, registerWalkOfInterestCommandResult.Value)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId),
                      ManufacturerId = manufacturerId,
                      NamingInfo = namingInfo,
                      MarkingInfo = markingInfo,
                      LifecycleInfo = new LifecycleInfo(DeploymentStateEnum.InService, null, null)
                  };

                  var placeSpanEquipmentResult = commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand).Result;

                  // Unregister interest if place span equipment failed
                  if (placeSpanEquipmentResult.IsFailed)
                  {
                      var unregisterCommandResult = commandDispatcher.HandleAsync<UnregisterInterest, Result>(new UnregisterInterest(walkOfInterestId)).Result;

                      if (unregisterCommandResult.IsFailed)
                          return new CommandResult(unregisterCommandResult);
                  }


                  return new CommandResult(placeSpanEquipmentResult);
                  
              }
            );


            Field<CommandResultType>(
              "affixSpanEquipmentToNodeContainer",
              description: "Affix a span equipment to a node container - i.e. to some condult closure, man hole etc.",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerId" },
                  new QueryArgument<NonNullGraphType<NodeContainerSideEnumType>> { Name = "nodeContainerSide" }
              ),
              resolve: context =>
              {
                  var spanSegmentId = context.GetArgument<Guid>("spanSegmentId");
                  var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");
                  var side = context.GetArgument<NodeContainerSideEnum>("nodeContainerSide");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var affixCommand = new AffixSpanEquipmentToNodeContainer(spanSegmentId, nodeContainerId, side)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var affixCommandResult = commandDispatcher.HandleAsync<AffixSpanEquipmentToNodeContainer, Result>(affixCommand).Result;

                  return new CommandResult(affixCommandResult);

              }
            );

            Field<CommandResultType>(
              "detachSpanEquipmentFromNodeContainer",
              description: "Detach a span equipment from a node container - i.e. from some condult closure, man hole etc.",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" }
              ),
              resolve: context =>
              {
                  var spanSegmentId = context.GetArgument<Guid>("spanSegmentId");
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var detachCommand = new DetachSpanEquipmentFromNodeContainer(spanSegmentId, routeNodeId)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var detachCommandResult = commandDispatcher.HandleAsync<DetachSpanEquipmentFromNodeContainer, Result>(detachCommand).Result;

                  return new CommandResult(detachCommandResult);

              }
            );


            Field<CommandResultType>(
              "cutSpanSegments",
              description: "Cut the span segments belonging to som span equipment at the route node specified",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentstoCut" }
              ),
              resolve: context =>
              {
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var spanSegmentToCut = context.GetArgument<Guid[]>("spanSegmentstoCut");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var cutCmd = new CutSpanSegmentsAtRouteNode(routeNodeId, spanSegmentToCut)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var cutResult = commandDispatcher.HandleAsync<CutSpanSegmentsAtRouteNode, Result>(cutCmd).Result;

                  return new CommandResult(cutResult);
              }
            );

            Field<CommandResultType>(
              "connectSpanSegments",
              description: "Connect the span segments belonging to two different span equipment at the route node specified",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentsToConnect" }
              ),
              resolve: context =>
              {
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var spanSegmentToConnect = context.GetArgument<Guid[]>("spanSegmentsToConnect");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var connectCmd = new ConnectSpanSegmentsAtRouteNode(routeNodeId, spanSegmentToConnect)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var connectResult = commandDispatcher.HandleAsync<ConnectSpanSegmentsAtRouteNode, Result>(connectCmd).Result;

                  return new CommandResult(connectResult);
              }
            );

            Field<CommandResultType>(
              "disconnectSpanSegments",
              description: "Disconnect two span segments belonging to two different span equipment at the route node specified",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNodeId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanSegmentsToDisconnect" }
              ),
              resolve: context =>
              {
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var spanSegmentToDisconnect = context.GetArgument<Guid[]>("spanSegmentsToDisconnect");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var disconnectCmd = new DisconnectSpanSegmentsAtRouteNode(routeNodeId, spanSegmentToDisconnect)
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var disconnectResult = commandDispatcher.HandleAsync<DisconnectSpanSegmentsAtRouteNode, Result>(disconnectCmd).Result;

                  return new CommandResult(disconnectResult);
              }
            );

            Field<CommandResultType>(
              "addAdditionalInnerSpanStructures",
              description: "Add inner span structures to an existing span equipment",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "spanStructureSpecificationIds" }
              ),
              resolve: context =>
              {
                  var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");
                  var specificationsId = context.GetArgument<Guid[]>("spanStructureSpecificationIds");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var addStructure = new PlaceAdditionalStructuresInSpanEquipment(
                    spanEquipmentId: spanEquipmentOrSegmentId,
                    structureSpecificationIds: specificationsId
                  )
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var addStructureResult = commandDispatcher.HandleAsync<PlaceAdditionalStructuresInSpanEquipment, Result>(addStructure).Result;

                  return new CommandResult(addStructureResult);
              }
            );


            Field<CommandResultType>(
              "removeSpanStructure",
              description: "Remove inner or outer span structure of a span equipment. When the outer span structure is removed the entire span equipment is removed from the network.",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId" }
              ),
              resolve: context =>
              {
                  var spanSegmentId = context.GetArgument<Guid>("spanSegmentId");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  var removeStructure = new RemoveSpanStructureFromSpanEquipment(
                    spanSegmentId: spanSegmentId
                  )
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var removeStructureResult = commandDispatcher.HandleAsync<RemoveSpanStructureFromSpanEquipment, Result>(removeStructure).Result;

                  return new CommandResult(removeStructureResult);
              }
            );


            Field<CommandResultType>(
              "move",
              description: "Move a span equipment / change its walk in the route network",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" },
                  new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>> { Name = "routeSegmentIds" }
              ),
              resolve: context =>
              {
                  var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");

                  var correlationId = Guid.NewGuid();

                  var userContext = context.UserContext as GraphQLUserContext;
                  var userName = userContext.Username;

                  // TODO: Get from work manager
                  var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                  Guid[] routeSegmentIds = context.GetArgument<Guid[]>("routeSegmentIds");

                  RouteNetworkElementIdList newWalkIds = new();
                  newWalkIds.AddRange(routeSegmentIds);

                  var moveCmd = new MoveSpanEquipment(
                    spanEquipmentId: spanEquipmentOrSegmentId,
                    newWalkIds: newWalkIds
                  )
                  {
                      CorrelationId = correlationId,
                      UserContext = new UserContext(userName, workTaskId)
                  };

                  var moveCmdResult = commandDispatcher.HandleAsync<MoveSpanEquipment, Result>(moveCmd).Result;

                  return new CommandResult(moveCmdResult);
              }
            );

            Field<CommandResultType>(
             "updateProperties",
             description: "Mutation that can be used to change the span equipment specification, manufacturer and/or marking information",
             arguments: new QueryArguments(
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" },
                 new QueryArgument<IdGraphType> { Name = "spanEquipmentSpecificationId" },
                 new QueryArgument<IdGraphType> { Name = "manufacturerId" },
                 new QueryArgument<MarkingInfoInputType> { Name = "markingInfo" }
             ),
             resolve: context =>
             {
                 var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");

                 var correlationId = Guid.NewGuid();

                 var userContext = context.UserContext as GraphQLUserContext;
                 var userName = userContext.Username;

                 // TODO: Get from work manager
                 var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");


                 var updateCmd = new UpdateSpanEquipmentProperties(spanEquipmentOrSegmentId: spanEquipmentOrSegmentId)
                 {
                     CorrelationId = correlationId,
                     UserContext = new UserContext(userName, workTaskId),
                     SpecificationId = context.HasArgument("spanEquipmentSpecificationId") ? context.GetArgument<Guid>("spanEquipmentSpecificationId") : null,
                     ManufacturerId = context.HasArgument("manufacturerId") ? context.GetArgument<Guid>("manufacturerId") : null,
                     MarkingInfo = context.HasArgument("markingInfo") ? context.GetArgument<MarkingInfo>("markingInfo") : null
                 };

                 var updateResult = commandDispatcher.HandleAsync<UpdateSpanEquipmentProperties, Result>(updateCmd).Result;

                 return new CommandResult(updateResult);
             }
           );
        }
    }
}
