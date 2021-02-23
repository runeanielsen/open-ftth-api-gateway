using GraphQL;
using GraphQL.Types;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Mutations
{
    public class RouteNodeMutations : ObjectGraphType
    {
        public RouteNodeMutations()
        {
            Description = "Route node mutations";

            Field<RouteNetworkElementType>(
              "updateNamingInfo",
              description: "Mutation used to update the name and/or description of a route node",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<NamingInfoInputType>> { Name = "input" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");
                  var state = RouteNetworkFakeState.GetRouteNodeState(id);
                  state.NamingInfo = context.GetArgument<NamingInfo>("input");
                  return RouteNetworkFakeState.UpdateRouteNodeState(state);
              }
            );


            Field<RouteNetworkElementType>(
               "updateLifecyleInfo",
               description: "Mutation used to update the lifecycle related properties of a route node",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<LifecycleInfoInputType>> { Name = "input" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<Guid>("id");
                   var state = RouteNetworkFakeState.GetRouteNodeState(id);
                   state.LifecycleInfo = context.GetArgument<LifecycleInfo>("input");
                   return RouteNetworkFakeState.UpdateRouteNodeState(state);
               }
            );


            Field<RouteNetworkElementType>(
               "updateMappingInfo",
               description: "Mutation used to update mapping/digitizing info related properties of a route node",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<MappingInfoInputType>> { Name = "input" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<Guid>("id");
                   var state = RouteNetworkFakeState.GetRouteNodeState(id);
                   state.MappingInfo = context.GetArgument<MappingInfo>("input");
                   return RouteNetworkFakeState.UpdateRouteNodeState(state);
               }
            );

            Field<RouteNetworkElementType>(
              "updateSafetyInfo",
              description: "Mutation used to update safety information of a node.",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<SafetyInfoInputType>> { Name = "input" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");
                  var state = RouteNetworkFakeState.GetRouteNodeState(id);
                  state.SafetyInfo = context.GetArgument<SafetyInfo>("input");
                  return RouteNetworkFakeState.UpdateRouteNodeState(state);
              }
            );

            Field<RouteNetworkElementType>(
            "updateRouteNodeInfo",
            description: "Mutation used to update route node specific properties",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<RouteNodeInfoInputType>> { Name = "input" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<Guid>("id");
                var state = RouteNetworkFakeState.GetRouteNodeState(id);
                state.RouteNodeInfo = context.GetArgument<RouteNodeInfo>("input");
                return RouteNetworkFakeState.UpdateRouteNodeState(state);
            }
            );
        }
    }
}
