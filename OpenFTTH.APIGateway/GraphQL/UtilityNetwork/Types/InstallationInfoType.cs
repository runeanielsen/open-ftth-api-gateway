using GraphQL.Types;
using OpenFTTH.APIGateway.Util;
using OpenFTTH.CQRS;
using OpenFTTH.UtilityGraphService.Business.Graph.Projections;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class InstallationInfoType : ObjectGraphType<InstallationRecord>
    {
        public InstallationInfoType(IQueryDispatcher queryDispatcher, UTM32WGS84Converter coordinateConverter)
        {
            Field(x => x.InstallationId, type: typeof(StringGraphType)).Description("Utility installation id");
            Field(x => x.LocationRemark, type: typeof(StringGraphType)).Description("Utility installation id");
            Field(x => x.Status, type: typeof(StringGraphType)).Description("Utility installation status");
        }
    }
}
