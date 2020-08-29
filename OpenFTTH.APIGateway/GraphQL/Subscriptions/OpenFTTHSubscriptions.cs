using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Schemas;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Subscriptions
{
    public class OpenFTTHSubscriptions : ObjectGraphType
    {
        public OpenFTTHSubscriptions(ILogger<OpenFTTHSubscriptions> logger, IDependencyResolver resolver, RouteNetworkEventSubscription routeNetworkEventSubscription, GeographicalAreaUpdatedEventSubscription geographicalAreaUpdatedEventSubscription)
        {
            Description = "GraphQL API for subscriping to various events in the Open FTTH system";

            routeNetworkEventSubscription.AddFields(this);
            geographicalAreaUpdatedEventSubscription.AddFields(this);
        }
    }
}
