using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentConnectivityViewEndInfoType : ObjectGraphType<SpanEquipmentAZConnectivityViewEndInfo>
    {
        public SpanEquipmentConnectivityViewEndInfoType(ILogger<SpanEquipmentConnectivityViewEndInfoType> logger)
        {
            Field(x => x.ConnectedTo, type: typeof(StringGraphType)).Description("The equipment the terminal is connected to");
            Field(x => x.End, type: typeof(StringGraphType)).Description("Information about the end of the circuit");
        }
    }
}
