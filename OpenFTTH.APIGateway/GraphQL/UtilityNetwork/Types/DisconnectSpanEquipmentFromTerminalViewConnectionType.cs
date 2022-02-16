using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class DisconnectSpanEquipmentFromTerminalViewConnectionType : ObjectGraphType<DisconnectSpanEquipmentFromTerminalViewConnection>
    {
        public DisconnectSpanEquipmentFromTerminalViewConnectionType(ILogger<DisconnectSpanEquipmentFromTerminalViewConnectionType> logger)
        {
            Field(x => x.SegmentId, type: typeof(IdGraphType)).Description("Id of the span segment");
            Field(x => x.TerminalId, type: typeof(IdGraphType)).Description("Id of the terminal");
            Field(x => x.IsConnected, type: typeof(BooleanGraphType)).Description("If span segment (fiber) is connecte to terminal equipment");
            Field(x => x.TerminalEquipmentName, type: typeof(StringGraphType)).Description("Name of the terminal equipment");
            Field(x => x.TerminalStructureName, type: typeof(StringGraphType)).Description("Name of terminal equipment structure (slot/tray/card)");
            Field(x => x.TerminalName, type: typeof(StringGraphType)).Description("Name of terminal (pin/port)");
            Field(x => x.SpanStructurePosition, type: typeof(IntGraphType)).Description("Fiber number");
            Field(x => x.SpanStructureName, type: typeof(StringGraphType)).Description("Name of span structure (tube/fiber)");
            Field(x => x.End, type: typeof(StringGraphType)).Description("Where the fiber is heading/ending.");
        }
    }
}
