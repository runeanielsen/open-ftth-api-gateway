using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.Events.RouteNetwork.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types
{
    public class RouteNodeInfoType : ObjectGraphType<RouteNodeInfo>
    {
        public RouteNodeInfoType(ILogger<RouteNodeInfoType> logger)
        {
            Field(x => x.Kind, type: typeof(RouteNodeKindEnumType)).Description("The type of physical structure - i.e. cabinet, hand hole etc.");
            Field(x => x.Function, type: typeof(RouteNodeFunctionEnumType)).Description("The function this node has - i.e. a splice point, conduit closure etc.");
        }
    }

    public class RouteNodeInfoInputType : InputObjectGraphType<RouteNodeInfo>
    {
        public RouteNodeInfoInputType(ILogger<RouteNodeInfoInputType> logger)
        {
            Field(x => x.Kind, type: typeof(RouteNodeKindEnumType)).Description("The type of physical structure - i.e. cabinet, hand hole etc.");
            Field(x => x.Function, type: typeof(RouteNodeFunctionEnumType)).Description("The function this node has - i.e. a splice point, conduit closure etc.");
        }
    }

}
