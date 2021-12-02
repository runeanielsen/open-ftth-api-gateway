using GraphQL.Types;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentNamingMethodEnumType : EnumerationGraphType<TerminalEquipmentNamingMethodEnum>
    {
        public TerminalEquipmentNamingMethodEnumType()
        {
            Name = "TerminalEquipmentNamingMethodEnum";
            Description = @"How a set of terminal equipments should be named/numbered";
        }
    }
}
