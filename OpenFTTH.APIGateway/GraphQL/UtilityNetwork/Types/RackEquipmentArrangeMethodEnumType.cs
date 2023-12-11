using GraphQL.Types;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class RackEquipmentArrangeMethodEnumType : EnumerationGraphType<RackEquipmentArrangeMethodEnum>
    {
        public RackEquipmentArrangeMethodEnumType()
        {
            Name = "RackEquipmentArrangeMethodEnum";
            Description = @"How rack terminal equipments should arranged - up/down";
        }
    }
}
