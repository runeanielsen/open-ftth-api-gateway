using GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types
{
    public class RouteNodeKindEnumType : EnumerationGraphType<RouteNodeKindEnum>
    {
        public RouteNodeKindEnumType()
        {
            Name = "RouteNodeKindEnum";
            Description = @"The type of structure - i.e. cabinet, hand hole etc.";
        }
    }
}
