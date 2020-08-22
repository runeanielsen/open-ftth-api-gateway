using OpenFTTH.RouteNetworkService.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Test
{
    public static class RouteNetworkFakeState
    {
        private static Dictionary<Guid, RouteNodeQueryResult> _routeNodes = new Dictionary<Guid, RouteNodeQueryResult>();
        private static Dictionary<Guid, RouteSegmentQueryResult> _routeSegments = new Dictionary<Guid, RouteSegmentQueryResult>();


        public static RouteNodeQueryResult GetRouteNodeState(Guid id)
        {
            if (!_routeNodes.ContainsKey(id))
                _routeNodes.Add(id, new RouteNodeQueryResult() { RouteNodeId = id });

            return _routeNodes[id];
        }

        public static RouteNodeQueryResult UpdateRouteNodeState(RouteNodeQueryResult routeNode)
        {
            _routeNodes[routeNode.RouteNodeId] = routeNode;
            return routeNode;
        }

        public static RouteSegmentQueryResult GetRouteSegmentState(Guid id)
        {
            if (!_routeSegments.ContainsKey(id))
                _routeSegments.Add(id, new RouteSegmentQueryResult() { RouteSegmentId = id });

            return _routeSegments[id];
        }

        public static RouteSegmentQueryResult UpdateRouteSegmentState(RouteSegmentQueryResult routeSegment)
        {
            _routeSegments[routeSegment.RouteSegmentId] = routeSegment;
            return routeSegment;
        }

    }
}
