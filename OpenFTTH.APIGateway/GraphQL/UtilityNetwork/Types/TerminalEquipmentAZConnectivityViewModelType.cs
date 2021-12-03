using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentAZConnectivityViewModelType : ObjectGraphType<TerminalEquipmentAZConnectivityViewModel>
    {
        public TerminalEquipmentAZConnectivityViewModelType(ILogger<TerminalEquipmentAZConnectivityViewModelType> logger)
        {
            Field(x => x.ParentNodeStructures, type: typeof(ListGraphType<TerminalEquipmentConnectivityViewNodeStructureInfoType>)).Description("Non-null of the terminal equipment is contained within parent node structure(s) - i.e. racks.");
            Field(x => x.TerminalEquipments, type: typeof(ListGraphType<TerminalEquipmentConnectivityViewEquipmentInfoType>)).Description("The terminal equipment(s)");
        }
    }
}
