using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using OpenFTTH.APIGateway.GraphQL.Mutations;
using OpenFTTH.APIGateway.GraphQL.Queries;
using OpenFTTH.APIGateway.GraphQL.Subscriptions;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Subscriptions;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Schemas
{
    public class OpenFTTHSchema : Schema
    {
        public OpenFTTHSchema(IServiceProvider serviceProvider, OpenFTTHQueries queries, OpenFTTHSubscriptions subscriptions, OpenFTTHMutations mutations) : base(serviceProvider)
        {
            Query = queries;

            Subscription = subscriptions;

            Mutation = mutations;

            RegisterType<RouteNetworkEditOperationOccuredEventType>();
        }
    }
}
