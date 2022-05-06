using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalStructureSpecificationType : ObjectGraphType<TerminalStructureSpecification>
    {
        public TerminalStructureSpecificationType(ILogger<TerminalStructureSpecificationType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Category, type: typeof(StringGraphType)).Description("Category - i.e. splice closure, subrack etc");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Unique specification name");
            Field(x => x.ShortName, type: typeof(IntGraphType)).Description("Short name");
            Field(x => x.Description, type: typeof(StringGraphType)).Description("Long description");
            Field(x => x.Deprecated, type: typeof(BooleanGraphType)).Description("Whereas the type of terminal equipment is still in use");
            Field(x => x.ManufacturerRefs, type: typeof(ListGraphType<IdGraphType>)).Description("Manufacturer providing products of the the specification");
        }
    }
}
