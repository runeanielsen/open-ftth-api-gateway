using GraphQL.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Mutations
{
    public class RouteNodeMutations1 : ObjectGraphType
    {
        public RouteNodeMutations1()
        {
            Description = "Route node mutations";

            Field<RouteNodeType>(
              "updateNamingInfo",
              description: "Mutation used to update the name and/or description of a route node",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<StringGraphType> { Name = "name" },
                  new QueryArgument<StringGraphType> { Name = "description" }
              ),
              resolve: context =>
              {
                  var id = context.GetArgument<Guid>("id");

                  if (RouteNodeState.State.ContainsKey(id))
                  {
                      var state = RouteNodeState.State[id];

                      var newNamingInfoState = new NamingInfo(
                            name: context.HasArgument("name") ? context.GetArgument<string>("name") : state.NamingInfo.Name,
                            description: context.HasArgument("description") ? context.GetArgument<string>("description") : state.NamingInfo.Description
                      );

                      state.NamingInfo = newNamingInfoState;

                      return RouteNodeState.State[id];
                  }
                  else
                      return null;
              }
          );
        }

        

    }
}
