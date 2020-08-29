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
using OpenFTTH.Events.Geo;
using OpenFTTH.Events.RouteNetwork;
using Topos.Config;

namespace OpenFTTH.APIGateway.Workers
{
    public class GeographicalAreaUpdatedEventConsumer : BackgroundService
    {
        private readonly ILogger<GeographicalAreaUpdatedEventConsumer> _logger;
        private readonly IToposTypedEventObservable<ObjectsWithinGeographicalAreaUpdated> _eventDispatcher;
        private readonly KafkaSetting _kafkaSetting;

        private IDisposable _kafkaConsumer;

        public GeographicalAreaUpdatedEventConsumer(ILogger<GeographicalAreaUpdatedEventConsumer> logger, IOptions<KafkaSetting> kafkaSetting, IToposTypedEventObservable<ObjectsWithinGeographicalAreaUpdated> eventDispatcher)
        {
            _logger = logger;
            _kafkaSetting = kafkaSetting.Value;
            _eventDispatcher = eventDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting geographical area updated event consumer worker at: {time}", DateTimeOffset.Now);

            try
            {
                _kafkaConsumer = _eventDispatcher.Config("geographical_area_updated_event_" + Guid.NewGuid(), c => c.UseKafka(_kafkaSetting.Server))
                          .Logging(l => l.UseSerilog())
                          .Positions(p => p.StoreInFileSystem(_kafkaSetting.PositionFilePath))
                          .Topics(t => t.Subscribe(_kafkaSetting.GeographicalAreaUpdatedTopic))
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
