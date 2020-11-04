using DAX.EventProcessing.Dispatcher;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using OpenFTTH.APIGateway.GeographicalAreaUpdated.GraphQL.Types;
using OpenFTTH.Events.Geo;
using OpenFTTH.Events.RouteNetwork;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace OpenFTTH.APIGateway.RouteNetwork.GraphQL.Subscriptions
{
    public class GeographicalAreaUpdatedEventSubscription
    {
        private readonly IToposTypedEventObservable<ObjectsWithinGeographicalAreaUpdated> _toposTypedEventObserable;

        public GeographicalAreaUpdatedEventSubscription(IToposTypedEventObservable<ObjectsWithinGeographicalAreaUpdated> toposTypedEventObserable)
        {
            _toposTypedEventObserable = toposTypedEventObserable;
        }

        public void AddFields(ObjectGraphType objectGraphType)
        {
            objectGraphType.AddField(new EventStreamFieldType
            {
                Name = "geographicalAreaUpdatedEvents",
                Type = typeof(ObjectsWithinGeographicalAreaUpdatedType),
                Resolver = new FuncFieldResolver<ObjectsWithinGeographicalAreaUpdated>(ResolveEvent),
                Subscriber = new EventStreamResolver<ObjectsWithinGeographicalAreaUpdated>(SubscribeEvents)
            });
        }

        private ObjectsWithinGeographicalAreaUpdated ResolveEvent(IResolveFieldContext context)
        {
            return context.Source as ObjectsWithinGeographicalAreaUpdated;
        }

        private IObservable<ObjectsWithinGeographicalAreaUpdated> SubscribeEvents(IResolveEventStreamContext context)
        {
            return _toposTypedEventObserable.OnEvent;
        }
    }
}
