using OpenFTTH.Core;
using System;

namespace OpenFTTH.UtilityGraphService.API.Model.Trace
{
    public record RouteNetworkTraceResult : IIdentifiedObject
    {
        public Guid Id { get; }
        public Guid FromRouteNodeId { get; }
        public Guid ToRouteNodeId { get; }
        public Guid[] RouteSegmentIds { get; }

        public string? FromRouteNodeName { get; }
        public string? ToRouteNodeName { get; }

        public NodeNameType FromNodeNameType { get; }
        public NodeNameType ToNodeNameType { get; }

        public string[] RouteSegmentGeometries { get; }

        public string? Name => null;

        public string? Description => null;

        public RouteNetworkTraceResult(Guid id, Guid fromRouteNodeId, Guid toRouteNodeId, Guid[] routeSegmentIds, string? fromRouteNodeName, string? toRouteNodeName, NodeNameType fromNodeNameType, NodeNameType toNodeNameType, string[] routeSegmentGeometries)
        {
            Id = id;
            FromRouteNodeId = fromRouteNodeId;
            ToRouteNodeId = toRouteNodeId;
            RouteSegmentIds = routeSegmentIds;
            FromRouteNodeName = fromRouteNodeName;
            ToRouteNodeName = toRouteNodeName;
            FromNodeNameType = fromNodeNameType;
            ToNodeNameType = toNodeNameType;
            RouteSegmentGeometries = routeSegmentGeometries;
        }
    }

    public enum NodeNameType
    {
        NODE_NAME = 1,
        ADDRESS = 2
    }
}
