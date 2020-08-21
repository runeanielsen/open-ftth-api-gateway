using GraphQL.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Mutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Mutations
{

    public class OpenFTTHMutations : ObjectGraphType
    {
        public OpenFTTHMutations()
        {
            Description = "Entry point for sending mutations to the various underlying services";

            Field<RouteNodeMutations>("routeNode", resolve: context => new { });
            //Field<RouteNodeMutations2>("routeNodeMutations_test2", resolve: context => new { });
        }
    }

}
