using OpenFTTH.RouteNetworkService.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test
{
    public static class RouteNodeState
    {
        public static Dictionary<Guid, RouteNodeQueryResult> State = new Dictionary<Guid, RouteNodeQueryResult>()
        {
            {
                Guid.Parse("20116798-dfa6-4311-a99a-a60ed0dd7097"),
                new RouteNodeQueryResult()
                {
                    RouteNodeId = Guid.Parse("20116798-dfa6-4311-a99a-a60ed0dd7097"),
                    NamingInfo = new Events.Core.Infos.NamingInfo("test obj 1 name", "test obj 1 description")
                }
            },
            {
                Guid.Parse("4eac7486-523f-4373-8cd4-57bab1336721"),
                new RouteNodeQueryResult()
                {
                    RouteNodeId = Guid.Parse("20116798-dfa6-4311-a99a-a60ed0dd7097"),
                    NamingInfo = new Events.Core.Infos.NamingInfo("test obj 2 name", "test obj 2 description")
                }
            },
        };
    }
}
