using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using DAX.EventProcessing.Dispatcher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.Events.RouteNetwork;
using Topos.Config;

namespace OpenFTTH.APIGateway.Workers
{
    public class RouteNetworkEventConsumer : BackgroundService
    {
        private readonly ILogger<RouteNetworkEventConsumer> _logger;
        private readonly IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent> _eventDispatcher;
        private readonly KafkaSetting _kafkaSetting;

        private IDisposable _kafkaConsumer;

        public RouteNetworkEventConsumer(ILogger<RouteNetworkEventConsumer> logger, IOptions<KafkaSetting> kafkaSetting, IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent> eventDispatcher)
        {
            _logger = logger;
            _kafkaSetting = kafkaSetting.Value;
            _eventDispatcher = eventDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting route network event consumer worker at: {time}", DateTimeOffset.Now);

            try
            {

                _kafkaConsumer = _eventDispatcher.Config("route_network_event_" + Guid.NewGuid(), c => c.UseKafka(_kafkaSetting.Server))
                              .Logging(l => l.UseSerilog())
                              .Positions(p => {
                                  p.StoreInFileSystem(_kafkaSetting.PositionFilePath);
                                  p.SetInitialPosition(StartFromPosition.Now);
                                }
                              )
                              .Topics(t => t.Subscribe(_kafkaSetting.RouteNetworkEventTopic))
                              .Start();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
