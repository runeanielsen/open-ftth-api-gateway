using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.Tests.Util
{
    public class UtilityNetworkBuilder
    {
        private readonly IEventStore _eventStore;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public UtilityNetworkBuilder(IEventStore eventStore, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _eventStore = eventStore;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<Result> ConnectTwoSpanSegmentsAsync(Guid nodeId, Guid fromSpanEquipmentId, int fromSpanStructureIndex, int fromSpanSegmentIndex, Guid toSpanEquipmentId, int toSpanStructureIndex, int toSpanSegmentIndex)
        {
            var utilityNetwork = _eventStore.Projections.Get<UtilityNetworkProjection>();

            utilityNetwork.TryGetEquipment<SpanEquipment>(fromSpanEquipmentId, out var fromSpanEquipment);
            utilityNetwork.TryGetEquipment<SpanEquipment>(toSpanEquipmentId, out var toSpanEquipment);

            var connectCmd = new ConnectSpanSegmentsAtRouteNode(Guid.NewGuid(), new UserContext("test", Guid.Empty),
                routeNodeId: nodeId,
                spanSegmentsToConnect: new Guid[] {
                    fromSpanEquipment.SpanStructures[fromSpanStructureIndex].SpanSegments[fromSpanSegmentIndex].Id,
                    toSpanEquipment.SpanStructures[toSpanStructureIndex].SpanSegments[toSpanSegmentIndex].Id
                }
            );

            var result = await _commandDispatcher.HandleAsync<ConnectSpanSegmentsAtRouteNode, Result>(connectCmd);

            if (result.IsFailed)
                throw new ApplicationException(result.Errors.First().Message);

            return result;
        }


    }
}
