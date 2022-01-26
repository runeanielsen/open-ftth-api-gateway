using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentAZConnectivityViewModelType : ObjectGraphType<SpanEquipmentAZConnectivityViewModel>
    {
        public SpanEquipmentAZConnectivityViewModelType(ILogger<SpanEquipmentAZConnectivityViewModelType> logger)
        {
            Field(x => x.SpanEquipments, type: typeof(ListGraphType<SpanEquipmentConnectivityViewEquipmentInfoType>)).Description("The span equipment(s)");
        }
    }
}
