using OpenFTTH.CQRS;
using OpenFTTH.Results;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using System;

namespace OpenFTTH.UtilityGraphService.API.Commands
{
    public record UpdateTags : BaseCommand, ICommand<Result>
    {
        public Guid NodeId { get; }
        public Guid TerminalOrSpanEquipmentId { get; }
        public EquipmentTag[] Tags { get; }

        public UpdateTags(Guid correlationId, UserContext userContext, Guid terminalOrSpanEquipmentId, EquipmentTag[] tags, Guid nodeId) : base(correlationId, userContext)
        {
            NodeId = nodeId;
            TerminalOrSpanEquipmentId = terminalOrSpanEquipmentId;
            Tags = tags;
        }
    }
}
