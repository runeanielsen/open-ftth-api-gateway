using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class ConnectivityFaceType : ObjectGraphType<ConnectivityFace>
    {
        public ConnectivityFaceType(ILogger<ConnectivityFaceType> logger)
        {
            Field(x => x.EquipmentId, type: typeof(IdGraphType)).Description("Terminal or span equipment id");
            Field(x => x.EquipmentName, type: typeof(StringGraphType)).Description("Equipment name");
            Field(x => x.EquipmentKind, type: typeof(ConnectivityEquipmentKindEnumType)).Description("Equipment kind");
            Field(x => x.FaceKind, type: typeof(FaceKindEnumType)).Description("Face kind");
            Field(x => x.FaceName, type: typeof(StringGraphType)).Description("Face name");
        }
    }
}
