using OpenFTTH.Results;
using OpenFTTH.CQRS;
using System;

namespace OpenFTTH.RouteNetwork.API.Queries
{
    /// <summary>
    /// Used to find the shortest path between two route segments in the route network
    /// </summary>
    public class ShortestPathBetweenRouteSegments : IQuery<Result<ShortestPathBetweenRouteSegmentsResult>>
    {
        public static string RequestName => typeof(ShortestPathBetweenRouteSegmentsResult).Name;
        public Guid SourceRouteSegmentId { get; }
        public Guid DestRouteSegmentId { get; }

        public ShortestPathBetweenRouteSegments(Guid sourceRouteSegmentId, Guid destRouteSegmentId)
        {
            SourceRouteSegmentId = sourceRouteSegmentId;
            DestRouteSegmentId = destRouteSegmentId;
        }
    }
}
