using GraphQL.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Mutations
{
    public class RouteNodeMutations2 : ObjectGraphType
    {
        public RouteNodeMutations2()
        {
            Description = "Route node mutations test 2";

            Field<RouteNodeType>(
              "updateNamingInfo",
              description: "Mutation used to update the name and/or description of a route node",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" },
                  new QueryArgument<NonNullGraphType<NamingInfoType>> { Name = "input" }
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
        }

        

    }
}
