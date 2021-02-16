using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentSpecificationType : ObjectGraphType<SpanEquipmentSpecification>
    {
        public SpanEquipmentSpecificationType(ILogger<ManufacturerType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Category, type: typeof(StringGraphType)).Description("Category - i.e. Conduit, Fiber Cable etc.");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Short name");
            Field(x => x.Description, type: typeof(StringGraphType)).Description("Long description");
            Field(x => x.Deprecated, type: typeof(StringGraphType)).Description("Whereas the manufacturer is still used in new projects");
            Field(x => x.ManufacturerRefs, type: typeof(ListGraphType<IdGraphType>)).Description("Manufacturer providing products of the the specification");
        }
    }
}
