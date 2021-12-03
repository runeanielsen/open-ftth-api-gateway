﻿using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentConnectivityViewTerminalStructureInfoType : ObjectGraphType<TerminalEquipmentConnectivityViewTerminalStructureInfo>
    {
        public TerminalEquipmentConnectivityViewTerminalStructureInfoType(ILogger<TerminalEquipmentConnectivityViewTerminalStructureInfoType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name of the node structure");
            Field(x => x.Category, type: typeof(StringGraphType)).Description("Category");
            Field(x => x.Info, type: typeof(StringGraphType)).Description("Additional information (remark)");
            Field(x => x.SpecName, type: typeof(StringGraphType)).Description("Specification name");
            Field(x => x.Lines, type: typeof(ListGraphType<TerminalEquipmentConnectivityViewTerminalStructureInfoType>)).Description("The terminal structures(s) - i.e. cards, trays etc.");
        }
    }
}
