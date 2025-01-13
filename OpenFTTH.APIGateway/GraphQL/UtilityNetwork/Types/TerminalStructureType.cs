using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.DynamicProperties;
using OpenFTTH.CQRS;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalStructureType : ObjectGraphType<TerminalStructure>
    {
        public TerminalStructureType(ILogger<TerminalStructureType> logger, IQueryDispatcher queryDispatcher, DynamicPropertiesClient dynamicPropertiesReader)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Short name");
            Field(x => x.Description, type: typeof(StringGraphType)).Description("Long description");
            Field(x => x.Position, type: typeof(IntGraphType)).Description("Position of tray/card");
            Field(x => x.SpecificationId, type: typeof(IdGraphType)).Description("Terminal structure specification id");
            Field(x => x.interfaceInfo, type: typeof(InterfaceInfoType)).Description("Interface info");
        }
    }
}
