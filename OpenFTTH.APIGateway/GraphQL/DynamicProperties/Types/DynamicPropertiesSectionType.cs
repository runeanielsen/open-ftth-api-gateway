using GraphQL.Types;
using OpenFTTH.APIGateway.DynamicProperties;

namespace OpenFTTH.APIGateway.GraphQL.DynamicProperties.Types
{
    public class DynamicPropertiesSectionType : ObjectGraphType<DynamicPropertiesSection>
    {
        public DynamicPropertiesSectionType()
        {
            Field(x => x.SectionName, type: typeof(StringGraphType)).Description("Name of the section");
            Field(x => x.Properties, type: typeof(ListGraphType<DynamicPropertyType>)).Description("All properties within the section");
        }
    }
}
