using DAX.EventProcessing.Dispatcher;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.Events.RouteNetwork;
using System;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Subscriptions
{
    public class RouteNetworkEventSubscription
    {
        private readonly IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent> _toposTypedEventObserable;

        public RouteNetworkEventSubscription(IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent> toposTypedEventObserable)
        {
            _toposTypedEventObserable = toposTypedEventObserable;
        }

        public void AddFields(ObjectGraphType objectGraphType)
        {
            objectGraphType.AddField(new EventStreamFieldType
            {
                Name = "routeEvents",
                Type = typeof(RouteNetworkEditOperationOccuredEventType),
                Resolver = new FuncFieldResolver<RouteNetworkEditOperationOccuredEvent>(ResolveEvent),
                Subscriber = new EventStreamResolver<RouteNetworkEditOperationOccuredEvent>(SubscribeEvents)
            });
        }

        private RouteNetworkEditOperationOccuredEvent ResolveEvent(IResolveFieldContext context)
        {
            return context.Source as RouteNetworkEditOperationOccuredEvent;
        }

        private IObservable<RouteNetworkEditOperationOccuredEvent> SubscribeEvents(IResolveEventStreamContext context)
        {
            return _toposTypedEventObserable.OnEvent;
        }
    }
}
