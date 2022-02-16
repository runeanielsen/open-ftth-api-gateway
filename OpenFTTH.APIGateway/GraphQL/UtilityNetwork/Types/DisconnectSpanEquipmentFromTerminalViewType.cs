using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class DisconnectSpanEquipmentFromTerminalViewType : ObjectGraphType<DisconnectSpanEquipmentFromTerminalView>
    {
        public DisconnectSpanEquipmentFromTerminalViewType(ILogger<DisconnectSpanEquipmentFromTerminalViewType> logger)
        {
            Field(x => x.SpanEquipmentName, type: typeof(StringGraphType)).Description("Name of the cable span equipment");
            Field(x => x.Lines, type: typeof(ListGraphType<DisconnectSpanEquipmentFromTerminalViewConnectionType>)).Description("One line for each fiber in the fiber cable connected to the terminal equipment");
        }
    }
}
