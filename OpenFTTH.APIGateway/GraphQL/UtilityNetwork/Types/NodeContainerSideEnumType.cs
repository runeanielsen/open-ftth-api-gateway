using GraphQL.Types;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class NodeContainerSideEnumType : EnumerationGraphType<NodeContainerSideEnum>
    {
        public NodeContainerSideEnumType()
        {
            Name = "NodeContainerSideEnum";
        }
    }
}
