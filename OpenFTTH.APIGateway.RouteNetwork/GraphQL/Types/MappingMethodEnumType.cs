using GraphQL.Types;
using OpenFTTH.Events.Core.Infos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types
{
    public class MappingMethodEnumType : EnumerationGraphType<MappingMethodEnum>
    {
        public MappingMethodEnumType()
        {
            Name = "MappingMethodEnum";
            Description = @"How the asset was digitized geographically.";
        }
    }
}
