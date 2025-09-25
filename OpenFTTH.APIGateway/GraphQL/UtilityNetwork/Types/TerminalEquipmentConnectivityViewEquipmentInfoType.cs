using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentConnectivityViewEquipmentInfoType : ObjectGraphType<TerminalEquipmentAZConnectivityViewEquipmentInfo>
    {
        public TerminalEquipmentConnectivityViewEquipmentInfoType(ILogger<TerminalEquipmentConnectivityViewEquipmentInfoType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.ParentNodeStructureId, type: typeof(IdGraphType)).Description("Parent structure id if any");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name of the node structure");
            Field(x => x.Category, type: typeof(StringGraphType)).Description("Category");
            Field(x => x.Info, type: typeof(StringGraphType)).Description("Additional information (remark)");
            Field(x => x.SpecName, type: typeof(StringGraphType)).Description("Specification name");
            Field(x => x.TerminalStructures, type: typeof(ListGraphType<TerminalEquipmentConnectivityViewTerminalStructureInfoType>)).Description("The terminal structures(s) - i.e. cards, trays etc.");
            Field(x => x.IsLineTermination, type: typeof(BooleanGraphType)).Description("Whether equipment is a line termination (OLT)");
            Field(x => x.TerminalStructuresIsNameable, type: typeof(BooleanGraphType)).Description("Whether the terminal structures (cards, trays etc) contained by the equipment are nameable");
        }
    }
}
