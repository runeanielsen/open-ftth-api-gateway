using FluentResults;
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
              "placSpanEquipmentInRouteNetwork",
              description: "Place a span equipment (i.e. conduit, cable whatwever) in the route network",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentId" },
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentSpecificationId" },
                  new QueryArgument<ListGraphType<IdGraphType>> { Name = "routeSegmentIds" },
                  new QueryArgument<MarkingInfoInputType> { Name = "markingInfo" },
                  new QueryArgument<NamingInfoInputType> { Name = "namingInfo" }
              ),
              resolve: context =>
              {
                  var spanEquipmentId = context.GetArgument<Guid>("spanEquipmentId");
                  var spanEquipmentSpecificationId = context.GetArgument<Guid>("spanEquipmentSpecificationId");
                  var routeSegmentIds = context.GetArgument<List<Guid>>("routeSegmentIds");
                  var markingInfo = context.GetArgument<MarkingInfo>("markingInfo");
                  var namingInfo = context.GetArgument<NamingInfo>("namingInfo");


                  var spanEquipments = eventStore.Projections.Get<SpanEquipmentsProjection>().SpanEquipments;

                  // First register the walk in the route network where the client want to place the span equipment
                  var walkOfInterestId = Guid.NewGuid();
                  var walk = new RouteNetworkElementIdList();
                  walk.AddRange(routeSegmentIds);
                  var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walk);
                  var registerWalkOfInterestCommandResult = commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand).Result;

                  if (registerWalkOfInterestCommandResult.IsFailed)
                  {
                      return new CommandResult(registerWalkOfInterestCommandResult);
                  }

                  // Now place the conduit in the walk
                  var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(spanEquipmentId, spanEquipmentSpecificationId, registerWalkOfInterestCommandResult.Value)
                  {
                      NamingInfo = namingInfo,
                      MarkingInfo = markingInfo
                  };

                  var placeSpanEquipmentResult = commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand).Result;

                  return new CommandResult(placeSpanEquipmentResult);
                  
              }
            );
          
        }
    }
}
