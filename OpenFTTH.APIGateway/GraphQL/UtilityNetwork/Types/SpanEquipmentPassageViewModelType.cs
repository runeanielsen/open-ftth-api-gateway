using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentPassageViewModelType : ObjectGraphType<SpanEquipmentPassageViewModel>
    {
        public SpanEquipmentPassageViewModelType(ILogger<SpanEquipmentPassageViewModelType> logger)
        {
            Field(x => x.SpanEquipments, type: typeof(ListGraphType<SpanEquipmentPassageViewEquipmentInfoType>)).Description("The span equipment(s)");
        }
    }
}
