using GraphQL.DataLoader;
using GraphQL.Types;
using OpenFTTH.Events.Geo;
using OpenFTTH.Events.RouteNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.GraphQL.Notifications.GeographicalAreaUpdated.Types
{
    public class ObjectsWithinGeographicalAreaUpdatedType : ObjectGraphType<ObjectsWithinGeographicalAreaUpdated>
    {
        public ObjectsWithinGeographicalAreaUpdatedType(IDataLoaderContextAccessor dataLoader)
        {
            Description = "Geographical area updated event has occured";

            Field(x => x.EventId, type: typeof(IdGraphType)).Description("Event id");
            Field(x => x.EventType, type: typeof(StringGraphType)).Description("Event type");
            Field(x => x.EventTimestamp, type: typeof(DateTimeGraphType)).Description("Event timestamp");
            Field(x => x.EventSequenceNumber, type: typeof(StringGraphType)).Description("Event sequence number");
            Field(x => x.Category, type: typeof(StringGraphType)).Description("Category of update - i.e RouteNetworkEditing, RouteNetworkValidation, Addresses etc.");
            Field(x => x.Envelope, type: typeof(EnvelopeType)).Description("Bouding box convering the objects updated.");
            Field(x => x.IdChangeSets, type: typeof(ListGraphType<IdChangeSetType>)).Description("Changes sets containing ids of the objects that has been added, modified or deleted");
        }
    }
}
