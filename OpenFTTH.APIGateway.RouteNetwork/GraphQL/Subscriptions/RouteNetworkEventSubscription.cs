using DAX.EventProcessing.Dispatcher;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.Events.RouteNetwork;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Subscriptions
{
    public class RouteNetworkEventSubscription
    {
        private readonly IToposTypedEventObservable<RouteNetworkEvent> _toposTypedEventObserable;

        public RouteNetworkEventSubscription(IToposTypedEventObservable<RouteNetworkEvent> toposTypedEventObserable)
        {
            _toposTypedEventObserable = toposTypedEventObserable;
        }

        public void AddFields(ObjectGraphType objectGraphType)
        {
            objectGraphType.AddField(new EventStreamFieldType
            {
                Name = "routeEvents",
                Type = typeof(RouteNetworkEventType),
                Resolver = new FuncFieldResolver<RouteNetworkEvent>(ResolveEvent),
                Subscriber = new EventStreamResolver<RouteNetworkEvent>(SubscribeEvents)
            });
        }

        private RouteNetworkEvent ResolveEvent(ResolveFieldContext context)
        {
            return context.Source as RouteNetworkEvent;
        }

        private IObservable<RouteNetworkEvent> SubscribeEvents(ResolveEventStreamContext context)
        {
            return _toposTypedEventObserable.OnEvent;
        }
    }
}
