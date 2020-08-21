﻿using GraphQL.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Mutations
{
    public class RouteNodeMutations : ObjectGraphType
    {
        public RouteNodeMutations()
        {
            Description = "Route node mutations";




            Field<RouteNodeType>(
              "updateNamingInfo",
              description: "Mutation used to update the name and/or description of a route node",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<NamingInfoInputType>> { Name = "input" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");

                  if (RouteNodeState.State.ContainsKey(id))
                  {
                      var state = RouteNodeState.State[id];

                      state.NamingInfo = context.GetArgument<NamingInfo>("input");

                      return RouteNodeState.State[id];
                  }
                  else
                      return null;
              }
            );


            Field<RouteNodeType>(
               "updateLifecyleInfo",
               description: "Mutation used to update the lifecycle related properties of a route node",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<LifecycleInfoInputType>> { Name = "input" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<Guid>("id");

                   if (RouteNodeState.State.ContainsKey(id))
                   {
                       var state = RouteNodeState.State[id];

                       state.LifecycleInfo = context.GetArgument<LifecycleInfo>("input");

                       return RouteNodeState.State[id];
                   }
                   else
                       return null;
               }
            );


            Field<RouteNodeType>(
               "updateMappingInfo",
               description: "Mutation used to update mapping/digitizing info related properties of a route node",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<MappingInfoInputType>> { Name = "input" }
               ),
               resolve: context =>
               {
                   var id = context.GetArgument<Guid>("id");

                   if (RouteNodeState.State.ContainsKey(id))
                   {
                       var state = RouteNodeState.State[id];

                       state.MappingInfo = context.GetArgument<MappingInfo>("input");

                       return RouteNodeState.State[id];
                   }
                   else
                       return null;
               }
            );

            Field<RouteNodeType>(
              "updateSafetyInfo",
              description: "Mutation used to update safety information of a node.",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<SafetyInfoInputType>> { Name = "input" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");

                  if (RouteNodeState.State.ContainsKey(id))
                  {
                      var state = RouteNodeState.State[id];

                      state.SafetyInfo = context.GetArgument<SafetyInfo>("input");

                      return RouteNodeState.State[id];
                  }
                  else
                      return null;
              }
            );

            Field<RouteNodeType>(
            "updateRouteNodeInfo",
            description: "Mutation used to update route node specific properties",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                new QueryArgument<NonNullGraphType<RouteNodeInfoInputType>> { Name = "input" }
            ),
            resolve: context =>
            {
                var id = context.GetArgument<Guid>("id");

                if (RouteNodeState.State.ContainsKey(id))
                {
                    var state = RouteNodeState.State[id];

                    state.RouteNodeInfo = context.GetArgument<RouteNodeInfo>("input");

                    return RouteNodeState.State[id];
                }
                else
                    return null;
            }
            );



        }
    }
}
