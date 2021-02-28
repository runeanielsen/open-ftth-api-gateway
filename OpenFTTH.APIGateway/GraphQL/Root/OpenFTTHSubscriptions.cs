using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Subscriptions;
using OpenFTTH.APIGateway.GraphQL.Schematic.Subscriptions;
using OpenFTTH.APIGateway.Notifications.GeographicalAreaUpdated.Subscriptions;

namespace OpenFTTH.APIGateway.GraphQL.Root
{
    public class OpenFTTHSubscriptions : ObjectGraphType
    {
        public OpenFTTHSubscriptions(ILogger<OpenFTTHSubscriptions> logger, RouteNetworkEventSubscription routeNetworkEventSubscription, GeographicalAreaUpdatedEventSubscription geographicalAreaUpdatedEventSubscription, SchematicUpdatedSubscription schematicUpdatedSubscription)
        {
            Description = "GraphQL API for subscriping to various events in the Open FTTH system";

            routeNetworkEventSubscription.AddFields(this);
            geographicalAreaUpdatedEventSubscription.AddFields(this);
            schematicUpdatedSubscription.AddFields(this);
        }
    }
}
