using GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types
{
    public class RouteNodeFunctionEnumType : EnumerationGraphType<RouteNodeFunctionEnum>
    {
        public RouteNodeFunctionEnumType()
        {
            Name = "RouteNodeFunctionEnum";
            Description = @"The function of the node- i.e. splice point, central office etc.";
        }
    }
}
