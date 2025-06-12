using System;
using System.Collections.Generic;

namespace OpenFTTH.RouteNetwork.API.Queries
{
    public record ShortestPathBetweenRouteSegmentsResult
    {
        public List<Guid> RouteNetworkElementIds { get; }

        public long ElapsedTimeMs { get; }

        public ShortestPathBetweenRouteSegmentsResult(List<Guid> routeNetworkElementIds, long elapsedTimeMs)
        {
            RouteNetworkElementIds = routeNetworkElementIds;
            ElapsedTimeMs = elapsedTimeMs;
        }
    }
}
