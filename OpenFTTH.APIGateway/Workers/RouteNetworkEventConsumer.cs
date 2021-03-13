using DAX.EventProcessing.Dispatcher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.Events.RouteNetwork;
using OpenFTTH.RouteNetwork.Business.RouteElements.EventHandling;
using OpenFTTH.RouteNetwork.Business.RouteElements.StateHandling;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Topos.Config;
using Topos.InMem;

namespace OpenFTTH.APIGateway.Workers
{
    public class RouteNetworkEventConsumer : BackgroundService
    {
        private readonly ILogger<RouteNetworkEventConsumer> _logger;
        private readonly IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent> _eventDispatcher;
        private readonly RouteNetworkEventHandler _routeNetworkEventHandler;
        private readonly IRouteNetworkState _routeNetworkState;
        private readonly KafkaSetting _kafkaSetting;
        private InMemPositionsStorage _positionsStorage = new InMemPositionsStorage();
        private IDisposable _kafkaConsumer;

        public RouteNetworkEventConsumer(ILogger<RouteNetworkEventConsumer> logger, IOptions<KafkaSetting> kafkaSetting, IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent> eventDispatcher, RouteNetworkEventHandler routeNetworkEventHandler, IRouteNetworkState routeNetworkState)
        {
            _logger = logger;
            _kafkaSetting = kafkaSetting.Value;
            _eventDispatcher = eventDispatcher;
            _routeNetworkEventHandler = routeNetworkEventHandler;
            _routeNetworkState = routeNetworkState;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting route network event consumer worker at: {time}", DateTimeOffset.Now);

            try
            {
                if (String.IsNullOrEmpty(_kafkaSetting.RouteNetworkEventTopic))
                    throw new ApplicationException("RouteNetworkEventTopic Kafka app setting must be set!");

                var toposConfig = _eventDispatcher.Config("route_network_event_" + Guid.NewGuid(), c => {
                    var kafkaConfig = c.UseKafka(_kafkaSetting.Server);

                    if (_kafkaSetting.CertificateFilename != null)
                    {
                        kafkaConfig.WithCertificate(_kafkaSetting.CertificateFilename);
                    }
                })
               .Logging(l => l.UseSerilog())
               .Positions(p => p.StoreInMemory(_positionsStorage))
               .Topics(t => t.Subscribe(_kafkaSetting.RouteNetworkEventTopic));

                _eventDispatcher.OnEvent.Subscribe(_routeNetworkEventHandler);

                ((InMemRouteNetworkState)_routeNetworkState).FinishLoadMode();

                _kafkaConsumer = toposConfig.Start();

                var inMemRouteNetworkState = (InMemRouteNetworkState)_routeNetworkState;


                // Wait for load mode to create an initial version/state
                _logger.LogInformation("Starting route network events load mode...");

                bool loadFinish = false;
                while (!stoppingToken.IsCancellationRequested && !loadFinish)
                {
                    _logger.LogDebug("Waiting for load mode to finish creating initial state...");

                    _logger.LogInformation($"{inMemRouteNetworkState.NumberOfObjectsLoaded} objects loaded.");

                    DateTime waitStartTimestamp = DateTime.UtcNow;

                    await Task.Delay(5000, stoppingToken);

                    TimeSpan timespan = waitStartTimestamp - inMemRouteNetworkState.LastEventRecievedTimestamp;

                    if (timespan.TotalSeconds > 10)
                    {
                        loadFinish = true;
                    }
                }

                ((InMemRouteNetworkState)_routeNetworkState).FinishLoadMode();
                _logger.LogInformation("Loading of initial route network state finished.");

                // We are now ready to serve the public if the loaded objects are bigger than 0
                if (inMemRouteNetworkState.NumberOfObjectsLoaded > 0)
                    File.Create("/tmp/healthy");
                else
                    throw new ApplicationException("Recieved no route network elements from Kafka topic.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
                        
            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping background worker");
            _kafkaConsumer.Dispose();

            await Task.CompletedTask;
        }

    }
}
