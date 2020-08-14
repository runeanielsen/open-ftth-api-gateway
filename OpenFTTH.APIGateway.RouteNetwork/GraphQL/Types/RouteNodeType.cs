using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.RouteNetworkService.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types
{
    public class RouteNodeType : ObjectGraphType<RouteNodeQueryResult>
    {
        public RouteNodeType(ILogger<RouteNodeType> logger)
        {
            Field(x => x.RouteNodeId, type: typeof(IdGraphType)).Description("Guid property");
            Field(x => x.NamingInfo, type: typeof(NamingInfoType)).Description("Asset info");
        }
    }
 }
