using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.CQRS;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SubrackPlacementInfoType : ObjectGraphType<SubrackPlacementInfo>
    {
        public SubrackPlacementInfoType(ILogger<SubrackPlacementInfoType> logger, IQueryDispatcher queryDispatcher)
        {
            Field(x => x.RackId, type: typeof(IdGraphType)).Description("Rack id");
            Field(x => x.StartUnitPosition, type: typeof(IntGraphType)).Description("Subrack start position (rack unit)");
            Field(x => x.PlacmentMethod, type: typeof(SubrackPlacmentMethodType)).Description("Subrack placement method");
        }



    }

    public class SubrackPlacementInfoInputType : InputObjectGraphType<SubrackPlacementInfo>
    {
        public SubrackPlacementInfoInputType(ILogger<SubrackPlacementInfoInputType> logger, IQueryDispatcher queryDispatcher)
        {
            Field(x => x.RackId, type: typeof(IdGraphType)).Description("Rack id");
            Field(x => x.StartUnitPosition, type: typeof(IntGraphType)).Description("Subrack start position (rack unit)");
            Field(x => x.PlacmentMethod, type: typeof(SubrackPlacmentMethodType)).Description("Subrack placement method");
        }
    }
}
