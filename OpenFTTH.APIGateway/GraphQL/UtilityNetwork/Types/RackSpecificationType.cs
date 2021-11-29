using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class RackSpecificationType : ObjectGraphType<RackSpecification>
    {
        public RackSpecificationType(ILogger<ManufacturerType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Specification name");
            Field(x => x.ShortName, type: typeof(StringGraphType)).Description("Short name");
            Field(x => x.Description, type: typeof(StringGraphType)).Description("Long description");
            Field(x => x.Deprecated, type: typeof(BooleanGraphType)).Description("Whereas the rack type is still in use");
        }
    }
}
