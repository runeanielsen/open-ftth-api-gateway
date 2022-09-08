using GraphQL.Types;
using OpenFTTH.APIGateway.DynamicProperties;

namespace OpenFTTH.APIGateway.GraphQL.DynamicProperties.Types
{
    public class DynamicPropertyType : ObjectGraphType<DynamicProperty>
    {
        public DynamicPropertyType()
        {
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Property name");
            Field(x => x.Value, type: typeof(StringGraphType)).Description("Property value");
        }
    }
}
