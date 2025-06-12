using System;
using System.Collections.Generic;

namespace OpenFTTH.RouteNetwork.API.Queries
{
    public record ShortestPathBetweenRouteSegmentsResult
    {
        public List<Guid> RouteNetworkElementIds { get; }
        public List<Guid> RouteSegmentElementIds { get; }

        public long ElapsedTimeMs { get; }

        public ShortestPathBetweenRouteSegmentsResult(List<Guid> routeNetworkElementIds, List<Guid> routeSegmentElementIds, long elapsedTimeMs)
        {
            RouteNetworkElementIds = routeNetworkElementIds;
            RouteSegmentElementIds = routeSegmentElementIds;
            ElapsedTimeMs = elapsedTimeMs;
        }
    }
}
