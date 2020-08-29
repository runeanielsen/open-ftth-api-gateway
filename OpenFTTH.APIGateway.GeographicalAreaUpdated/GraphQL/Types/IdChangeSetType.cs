using GraphQL.Types;
using OpenFTTH.Events.Changes;
using OpenFTTH.Events.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.GeographicalAreaUpdated.GraphQL.Types
{
    public class IdChangeSetType :  ObjectGraphType<IdChangeSet>
    {
        public IdChangeSetType()
        {
            Field(x => x.ObjectType, type: typeof(StringGraphType)).Description("The type/class of object that the ids blongs to.");
            Field(x => x.ChangeType, type: typeof(ChangeTypeEnumType)).Description("Addition, Modification or Deletion");
            Field(x => x.IdList, type: typeof(ListGraphType<IdGraphType>)).Description("List of object guids");
        }
    }
}
