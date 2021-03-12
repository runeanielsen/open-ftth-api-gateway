using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanSegmentTrace
    {
        public Guid[] RouteNetworkSegmentIds { get; set; }
        public Guid[] RouteNetworkNodeIds { get; set; }
    }
}
