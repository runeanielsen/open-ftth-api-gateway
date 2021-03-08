using FluentResults;
using GraphQL;
using GraphQL.Types;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.CQRS;
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
                  var routeNodeId = context.GetArgument<Guid>("routeNodeId");
                  var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");
                  var nodeContainerSpecificationId = context.GetArgument<Guid>("nodeContainerSpecificationId");
                  var manufacturerId = context.GetArgument<Guid>("manufacturerId");


                  // First register the walk in the route network where the client want to place the node container
                  var nodeOfInterestId = Guid.NewGuid();
                  var walk = new RouteNetworkElementIdList();
                  var registerNodeOfInterestCommand = new RegisterNodeOfInterest(nodeOfInterestId, routeNodeId);
                  var registerNodeOfInterestCommandResult = commandDispatcher.HandleAsync<RegisterNodeOfInterest, Result<RouteNetworkInterest>>(registerNodeOfInterestCommand).Result;

                  if (registerNodeOfInterestCommandResult.IsFailed)
                  {
                      return new CommandResult(registerNodeOfInterestCommandResult);
                  }

                  // Now place the conduit in the walk
                  var placeNodeContainerCommand = new PlaceNodeContainerInRouteNetwork(nodeContainerId, nodeContainerSpecificationId, registerNodeOfInterestCommandResult.Value)
                  {
                      ManufacturerId = manufacturerId
                  };

                  var placeNodeContainerResult = commandDispatcher.HandleAsync<PlaceNodeContainerInRouteNetwork, Result>(placeNodeContainerCommand).Result;

                  return new CommandResult(placeNodeContainerResult);
              }
            );
        }
    }
}
