using GraphQL.Types;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SubrackPlacmentMethodType : EnumerationGraphType<SubrackPlacmentMethod>
    {
        public SubrackPlacmentMethodType()
        {
            Name = "SubrackPlacmentMethodEnum";
            Description = @"How terminal equipments should be put into the rack (top-down or bottom-up)";
        }
    }
}
