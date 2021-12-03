using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentAZConnectivityViewLineInfoType : ObjectGraphType<TerminalEquipmentAZConnectivityViewLineInfo>
    {
        public TerminalEquipmentAZConnectivityViewLineInfoType(ILogger<TerminalEquipmentAZConnectivityViewLineInfoType> logger)
        {
            Field(x => x.ConnectorSymbol, type: typeof(StringGraphType)).Description("Connector symbol");
            Field(x => x.A, type: typeof(TerminalEquipmentConnectivityViewEndInfoType)).Description("A info");
            Field(x => x.Z, type: typeof(TerminalEquipmentConnectivityViewEndInfoType)).Description("Z info");
        }
    }
}
