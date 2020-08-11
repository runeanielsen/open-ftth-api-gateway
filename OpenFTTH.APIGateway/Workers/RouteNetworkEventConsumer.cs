using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly IToposTypedEventObservable<RouteNetworkEvent> _eventDispatcher;

        private readonly IOptions<KafkaSetting> _kafkaSetting;

        public RouteNetworkEventConsumer(ILogger<RouteNetworkEventConsumer> logger, IOptions<KafkaSetting> kafkaSetting, IToposTypedEventObservable<RouteNetworkEvent> eventDispatcher)
        {
            _logger = logger;
            _kafkaSetting = kafkaSetting;
            _eventDispatcher = eventDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting route network event consumer worker at: {time}", DateTimeOffset.Now);

            _eventDispatcher.Config("route_network_event", c => c.UseKafka(_kafkaSetting.Value.Server))
                          .Logging(l => l.UseSerilog())
                          .Positions(p => p.StoreInFileSystem(_kafkaSetting.Value.PositionFilePath))
                          .Topics(t => t.Subscribe(_kafkaSetting.Value.RouteNetworkEventTopic))
                          .Start();
        }
    }
}
