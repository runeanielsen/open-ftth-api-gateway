using DAX.EventProcessing.Dispatcher;
using FluentResults;
using OpenFTTH.CQRS;
using OpenFTTH.Events.UtilityNetwork;
using OpenFTTH.Schematic.API.Model.DiagramLayout;
using OpenFTTH.Schematic.API.Queries;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace OpenFTTH.APIGateway.GraphQL.Schematic.Subscriptions
{
    public class SchematicDiagramObserver : IObserver<RouteNetworkElementContainedEquipmentUpdated>
    {
        private readonly IToposTypedEventObservable<RouteNetworkElementContainedEquipmentUpdated> _toposTypedEventObserable;
        private readonly IQueryDispatcher _queryDispatcher;

        private ConcurrentDictionary<Guid, Subject<Diagram>> _observableByRouteNetworkElementId = new ConcurrentDictionary<Guid, Subject<Diagram>>();

        public SchematicDiagramObserver(IToposTypedEventObservable<RouteNetworkElementContainedEquipmentUpdated> toposTypedEventObserable, IQueryDispatcher queryDispatcher)
        {
            _toposTypedEventObserable = toposTypedEventObserable;
            _queryDispatcher = queryDispatcher;
            _toposTypedEventObserable.OnEvent.Subscribe(this);
        }

        public IObservable<Diagram> WhenDiagramNeedsUpdate(Guid routeNetworkElementId)
        {
            return GetObservable(routeNetworkElementId).AsObservable();
        }

        private Subject<Diagram> GetObservable(Guid routeNetworkElementId)
        {
            return _observableByRouteNetworkElementId.GetOrAdd(routeNetworkElementId, new Subject<Diagram>());
        }

        public void Ping(Guid routeNetworkElementId)
        {
            if (_observableByRouteNetworkElementId.TryGetValue(routeNetworkElementId, out var observable))
            {
                observable.OnNext(GetDiagram(routeNetworkElementId));
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        void IObserver<RouteNetworkElementContainedEquipmentUpdated>.OnNext(RouteNetworkElementContainedEquipmentUpdated @event)
        {
            foreach (var routeNetworkElementId in @event.AffectedRouteNetworkElementIds)
            {
                if (_observableByRouteNetworkElementId.TryGetValue(routeNetworkElementId, out var observable))
                {
                    observable.OnNext(GetDiagram(routeNetworkElementId));
                }
            }
        }
        
        private Diagram GetDiagram(Guid routeNetworkElementId)
        {
            var getDiagramQueryResult = _queryDispatcher.HandleAsync<GetDiagram, Result<GetDiagramResult>>(new GetDiagram(routeNetworkElementId)).Result;

            if (getDiagramQueryResult.IsFailed)
            {
                throw new ApplicationException($"Schematic diagram creation for route node element with id: {routeNetworkElementId} failed with message: {getDiagramQueryResult.Errors.First().Message}");
            }

            return getDiagramQueryResult.Value.Diagram;
        }
    }
}
