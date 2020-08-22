using GraphQL.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Mutations
{
    public class RouteSegmentMutations : ObjectGraphType
    {
        public RouteSegmentMutations()
        {
            Description = "Route segment mutations";

            Field<RouteSegmentType>(
              "updateNamingInfo",
              description: "Mutation used to update the name and/or description of a route segment",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<NamingInfoInputType>> { Name = "input" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");
                  var state = RouteNetworkFakeState.GetRouteSegmentState(id);
                  state.NamingInfo = context.GetArgument<NamingInfo>("input");
                  return RouteNetworkFakeState.UpdateRouteSegmentState(state);
              }
            );


            Field<RouteSegmentType>(
               "updateLifecyleInfo",
               description: "Mutation used to update the lifecycle related properties of a route segment",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<LifecycleInfoInputType>> { Name = "input" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<Guid>("id");
                   var state = RouteNetworkFakeState.GetRouteSegmentState(id);
                   state.LifecycleInfo = context.GetArgument<LifecycleInfo>("input");
                   return RouteNetworkFakeState.UpdateRouteSegmentState(state);
               }
            );


            Field<RouteSegmentType>(
               "updateMappingInfo",
               description: "Mutation used to update mapping/digitizing info related properties of a route segment",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<MappingInfoInputType>> { Name = "input" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<Guid>("id");
                   var state = RouteNetworkFakeState.GetRouteSegmentState(id);
                   state.MappingInfo = context.GetArgument<MappingInfo>("input");
                   return RouteNetworkFakeState.UpdateRouteSegmentState(state);
               }
            );

            Field<RouteSegmentType>(
              "updateSafetyInfo",
              description: "Mutation used to update safety information of a segment.",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<SafetyInfoInputType>> { Name = "input" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");
                  var state = RouteNetworkFakeState.GetRouteSegmentState(id);
                  state.SafetyInfo = context.GetArgument<SafetyInfo>("input");
                  return RouteNetworkFakeState.UpdateRouteSegmentState(state);
              }
            );

            Field<RouteSegmentType>(
            "updateRouteSegmentInfo",
            description: "Mutation used to update route segment specific properties",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<RouteSegmentInfoInputType>> { Name = "input" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<Guid>("id");
                var state = RouteNetworkFakeState.GetRouteSegmentState(id);
                state.RouteSegmentInfo = context.GetArgument<RouteSegmentInfo>("input");
                return RouteNetworkFakeState.UpdateRouteSegmentState(state);
            }
            );
        }
    }
}
