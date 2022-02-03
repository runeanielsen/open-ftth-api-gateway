using GraphQL.Types;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class ConnectivityEquipmentKindEnumType : EnumerationGraphType<ConnectivityEquipmentKindEnum>
    {
        public ConnectivityEquipmentKindEnumType()
        {
            Name = "ConnectivityEquipmentKindEnumType";
            Description = @"What type of equipment (span equipment or terminal equipment)";
        }
    }
}
