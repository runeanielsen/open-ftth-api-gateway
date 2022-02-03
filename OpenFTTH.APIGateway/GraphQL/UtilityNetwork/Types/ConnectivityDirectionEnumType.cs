using GraphQL.Types;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class FaceKindEnumType : EnumerationGraphType<FaceKindEnum>
    {
        public FaceKindEnumType()
        {
            Name = "FaceKindEnumType";
            Description = @"Ingoing/upstream or outgoing/downstream face of connectivity";
        }
    }
}
