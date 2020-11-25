using GraphQL.Types;
using OpenFTTH.Events.Changes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.GraphQL.Notifications.GeographicalAreaUpdated.Types
{
    public class ChangeTypeEnumType : EnumerationGraphType<ChangeTypeEnum>
    {
        public ChangeTypeEnumType()
        {
            Name = "ChangeTypeEnum";
            Description = @"Type of changes - if it's adds, modifications or deletes";
        }
    }
}
