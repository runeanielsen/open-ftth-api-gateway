using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentConnectivityViewEndInfoType : ObjectGraphType<TerminalEquipmentAZConnectivityViewEndInfo>
    {
        public TerminalEquipmentConnectivityViewEndInfoType(ILogger<TerminalEquipmentConnectivityViewEndInfoType> logger)
        {
            Field(x => x.Terminal, type: typeof(TerminalEquipmentConnectivityViewTerminalInfoType)).Description("Terminal");
            Field(x => x.ConnectedTo, type: typeof(StringGraphType)).Description("The equipment the terminal is connected to");
            Field(x => x.End, type: typeof(StringGraphType)).Description("Information about the end of the circuit");
            Field(x => x.FaceKind, type: typeof(FaceKindEnumType)).Description("Connectivity face kind");
        }
    }
}
