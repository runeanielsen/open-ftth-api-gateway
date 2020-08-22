using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.RouteNetworkService.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types
{
    public class RouteSegmentType : ObjectGraphType<RouteSegmentQueryResult>
    {
        public RouteSegmentType(ILogger<RouteNodeType> logger)
        {
            Field(x => x.RouteSegmentId, type: typeof(IdGraphType)).Description("Guid property");

            Field(x => x.RouteSegmentInfo, type: typeof(RouteSegmentInfoType)).Description("Route node specific properties");
            Field(x => x.NamingInfo, type: typeof(NamingInfoType)).Description("Asset info");
            Field(x => x.LifecycleInfo, type: typeof(LifecycleInfoType)).Description("Lifecycle info");
            Field(x => x.MappingInfo, type: typeof(MappingInfoType)).Description("Mapping/digitizing method info");
            Field(x => x.SafetyInfo, type: typeof(SafetyInfoType)).Description("Safety info");
        }
    }
 }
