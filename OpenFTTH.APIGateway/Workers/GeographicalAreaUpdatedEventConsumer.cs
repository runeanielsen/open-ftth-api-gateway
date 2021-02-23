using DAX.EventProcessing.Dispatcher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.Events.Geo;
using System;
using System.Threading;
using System.Threading.Tasks;
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
                _kafkaConsumer = _eventDispatcher.Config("geographical_area_updated_event_" + Guid.NewGuid(), c => {
                    var kafkaConfig = c.UseKafka(_kafkaSetting.Server);

                    if (_kafkaSetting.CertificateFilename != null)
                    {
                        kafkaConfig.WithCertificate(_kafkaSetting.CertificateFilename);
                    }
                })
              .Logging(l => l.UseSerilog())
              .Positions(p => p.StoreInFileSystem(_kafkaSetting.PositionFilePath))
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
