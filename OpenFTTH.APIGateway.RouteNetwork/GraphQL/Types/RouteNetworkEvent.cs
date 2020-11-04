using GraphQL.DataLoader;
using GraphQL.Types;
using OpenFTTH.Events.RouteNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types
{
    public class RouteNetworkEditOperationOccuredEventType : ObjectGraphType<RouteNetworkEditOperationOccuredEvent>
    {
        public RouteNetworkEditOperationOccuredEventType(IDataLoaderContextAccessor dataLoader)
        {
            Description = "Route network event has occured";

            Field(x => x.EventId, type: typeof(IdGraphType)).Description("Event id");
            Field(x => x.EventType, type: typeof(StringGraphType)).Description("Event type");
        }
    }
}
