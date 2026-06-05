using DAX.EventProcessing;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Events.Changes;
using OpenFTTH.Events.UtilityNetwork;
using OpenFTTH.Results;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.Graph;
using OpenFTTH.UtilityGraphService.Business.NodeContainers;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments;
using OpenFTTH.UtilityGraphService.Business.TerminalEquipments.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.UtilityGraphService.Business.TerminalEquipments.CommandHandlers
{
    public class UpdateTagsCommandHandler : ICommandHandler<UpdateTags, Result>
    {
        private readonly IEventStore _eventStore;
        private readonly IExternalEventProducer _externalEventProducer;

        public UpdateTagsCommandHandler(IEventStore eventStore, IQueryDispatcher queryDispatcher, IExternalEventProducer externalEventProducer)
        {
            _eventStore = eventStore;
            _externalEventProducer = externalEventProducer;
        }

        public Task<Result> HandleAsync(UpdateTags command)
        {
            var utilityNetwork = _eventStore.Projections.Get<UtilityNetworkProjection>();

            // Because the client is allowed to provide either a span equipment or segment id, we need look it up via the utility network graph
            if (utilityNetwork.TryGetEquipment<TerminalEquipment>(command.TerminalOrSpanEquipmentId, out var terminalEquipment))
            {
                var terminalEquipmentAR = _eventStore.Aggregates.Load<TerminalEquipmentAR>(terminalEquipment.Id);

                var commandContext = new CommandContext(command.CorrelationId, command.CmdId, command.UserContext);

                var updateNamingInfoResult = terminalEquipmentAR.UpdateTags(
                    cmdContext: commandContext,
                    command.Tags
                );

                _eventStore.Aggregates.Store(terminalEquipmentAR);

                NotifyExternalServicesAboutSpanEquipmentChange(terminalEquipment.Id, command.NodeId);

                return Task.FromResult(Result.Ok());
            }
            else if (utilityNetwork.TryGetEquipment<SpanEquipment>(command.TerminalOrSpanEquipmentId, out var spanEquipment))
            {
                var spanEquipmentAR = _eventStore.Aggregates.Load<SpanEquipmentAR>(spanEquipment.Id);
                var commandContext = new CommandContext(command.CorrelationId, command.CmdId, command.UserContext);

                var updateNamingInfoResult = spanEquipmentAR.UpdateTags(
                    cmdContext: commandContext,
                    command.Tags
                );

                _eventStore.Aggregates.Store(spanEquipmentAR);

                NotifyExternalServicesAboutSpanEquipmentChange(spanEquipment.Id, command.NodeId);

                return Task.FromResult(Result.Ok());
            }
            else
                return Task.FromResult(Result.Fail(new UpdateEquipmentPropertiesError(UpdateEquipmentPropertiesErrorCodes.TERMINAL_OR_SPAN_EQUIPMENT_NOT_FOUND, $"Cannot find any terminal or span equipment in the utility graph with id: {command.TerminalOrSpanEquipmentId}")));

        }

        private async void NotifyExternalServicesAboutSpanEquipmentChange(Guid spanEquipmentId, Guid routeNodeId)
        {
            var idChangeSets = new List<IdChangeSet>
            {
                new IdChangeSet("TerminalEquipment", ChangeTypeEnum.Modification, new Guid[] { spanEquipmentId })
            };

            var updatedEvent =
                new RouteNetworkElementContainedEquipmentUpdated(
                    eventType: typeof(RouteNetworkElementContainedEquipmentUpdated).Name,
                    eventId: Guid.NewGuid(),
                    eventTimestamp: DateTime.UtcNow,
                    applicationName: "UtilityNetworkService",
                    applicationInfo: null,
                    category: "EquipmentModification.TagsUpdated",
                    idChangeSets: idChangeSets.ToArray(),
                    affectedRouteNetworkElementIds: new Guid[] { routeNodeId }
                );

            await _externalEventProducer.Produce(
                nameof(RouteNetworkElementContainedEquipmentUpdated),
                updatedEvent);
        }
    }
}
