using DAX.EventProcessing;
using OpenFTTH.Results;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Events.Changes;
using OpenFTTH.Events.UtilityNetwork;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.Graph;
using OpenFTTH.UtilityGraphService.Business.NodeContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.UtilityGraphService.Business.TerminalEquipments.CommandHandlers
{
    public class RemoveTerminalEquipmentCommandHandler : ICommandHandler<RemoveTerminalEquipment, Result>
    {
        private readonly IEventStore _eventStore;
        private readonly IExternalEventProducer _externalEventProducer;

        public RemoveTerminalEquipmentCommandHandler(IEventStore eventStore, IExternalEventProducer externalEventProducer)
        {
            _eventStore = eventStore;
            _externalEventProducer = externalEventProducer;
        }

        public Task<Result> HandleAsync(RemoveTerminalEquipment command)
        {
            var utilityNetwork = _eventStore.Projections.Get<UtilityNetworkProjection>();

            if (!utilityNetwork.TryGetEquipment<TerminalEquipment>(command.TerminalEquipmentId, out var terminalEquipment))
            {
                return Task.FromResult(Result.Fail(new RemoveTerminalEquipmentError(RemoveTerminalEquipmentErrorCodes.TERMINAL_EQUIPMENT_NOT_FOUND, $"Cannot find any terminal equipment with id: {command.TerminalEquipmentId}")));
            }

            if (!utilityNetwork.TryGetEquipment<NodeContainer>(terminalEquipment.NodeContainerId, out var nodeContainer))
            {
                throw new ApplicationException($"Error looking up node container by id: {terminalEquipment.NodeContainerId} referenced by terminal equipment with id: {terminalEquipment.Id}");
            }

            var terminalEquipmentAR = _eventStore.Aggregates.Load<TerminalEquipmentAR>(command.TerminalEquipmentId);

            var commandContext = new CommandContext(command.CorrelationId, command.CmdId, command.UserContext);

            // Remove the equipment
            var removeEquipmentResult = terminalEquipmentAR.Remove(commandContext, utilityNetwork.Graph);

            if (removeEquipmentResult.IsFailed)
            {
                return Task.FromResult(removeEquipmentResult);
            }

            // Remove reference from container
            var nodeContainerAR = _eventStore.Aggregates.Load<NodeContainerAR>(nodeContainer.Id);

            var removeEquipmentReferenceResult = nodeContainerAR.RemoveTerminalEquipmentReference(commandContext, command.TerminalEquipmentId);


            if (removeEquipmentReferenceResult.IsSuccess)
            {
                _eventStore.Aggregates.Store(terminalEquipmentAR);
                _eventStore.Aggregates.Store(nodeContainerAR);

                NotifyExternalServicesAboutChange(nodeContainer.RouteNodeId, terminalEquipment.Id);
            }

            return Task.FromResult(removeEquipmentReferenceResult);

        }

        private async void NotifyExternalServicesAboutChange(Guid routeNodeId, Guid terminalEquipmentId)
        {
            var idChangeSets = new List<IdChangeSet>
            {
                new IdChangeSet("TerminalEquipment", ChangeTypeEnum.Deletion, new Guid[] { terminalEquipmentId })
            };

            var updatedEvent =
                new RouteNetworkElementContainedEquipmentUpdated(
                    eventType: typeof(RouteNetworkElementContainedEquipmentUpdated).Name,
                    eventId: Guid.NewGuid(),
                    eventTimestamp: DateTime.UtcNow,
                    applicationName: "UtilityNetworkService",
                    applicationInfo: null,
                    category: "EquipmentDeletion",
                    idChangeSets: idChangeSets.ToArray(),
                    affectedRouteNetworkElementIds: new Guid[] { routeNodeId }
                );

            await _externalEventProducer.Produce(
                nameof(RouteNetworkElementContainedEquipmentUpdated),
                updatedEvent);
        }
    }
}
