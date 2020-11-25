using GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types
{
    public class RouteSegmentKindEnumType : EnumerationGraphType<RouteSegmentKindEnum>
    {
        public RouteSegmentKindEnumType()
        {
            Name = "RouteSegmentKindEnum";
            Description = @"The type of structure - i.e. cabinet, hand hole etc.";
        }
    }
}
