using System.Collections.Generic;

namespace OpenFTTH.APIGateway.DynamicProperties
{
    public class DynamicPropertiesSection
    {
        public string SectionName { get; }
        public List<DynamicProperty> Properties { get; set; }

        public DynamicPropertiesSection(string sectionName)
        {
            SectionName = sectionName;
            Properties = new();
        }
    }
}