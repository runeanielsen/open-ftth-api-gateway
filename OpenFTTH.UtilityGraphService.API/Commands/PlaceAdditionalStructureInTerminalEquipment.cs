using OpenFTTH.CQRS;
using OpenFTTH.Results;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using System;

namespace OpenFTTH.UtilityGraphService.API.Commands
{
    public record PlaceAdditionalStructureInTerminalEquipment : BaseCommand, ICommand<Result>
    {
        public Guid RouteNodeId { get; }
        public Guid TerminalEquipmentId { get; }
        public Guid StructureSpecificationId { get;  }
        public int Position { get; }
        public string Name { get; }
        public InterfaceInfo? InterfaceInfo { get; set; }

        public PlaceAdditionalStructureInTerminalEquipment(Guid correlationId, UserContext userContext, Guid routeNodeId, Guid terminalEquipmentId, Guid structureSpecificationId, int position, string name) : base(correlationId, userContext)
        {
            RouteNodeId = routeNodeId;
            TerminalEquipmentId = terminalEquipmentId;
            StructureSpecificationId = structureSpecificationId;
            Position = position;
            Name = name;
        }
    }
}
