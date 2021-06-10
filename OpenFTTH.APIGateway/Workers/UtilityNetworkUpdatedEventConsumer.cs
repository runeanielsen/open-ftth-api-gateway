using DAX.EventProcessing.Dispatcher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.Events.UtilityNetwork;
using System;
using System.Threading;
using System.Threading.Tasks;
using Topos.Config;

namespace OpenFTTH.APIGateway.Workers
{
    public class UtilityNetworkUpdatedEventConsumer : BackgroundService
    {
        private readonly ILogger<UtilityNetworkUpdatedEventConsumer> _logger;
        private readonly IToposTypedEventObservable<RouteNetworkElementContainedEquipmentUpdated> _eventDispatcher;
        private readonly KafkaSetting _kafkaSetting;

        private IDisposable _kafkaConsumer;

        public UtilityNetworkUpdatedEventConsumer(ILogger<UtilityNetworkUpdatedEventConsumer> logger, IOptions<KafkaSetting> kafkaSetting, IToposTypedEventObservable<RouteNetworkElementContainedEquipmentUpdated> eventDispatcher)
        {
            _logger = logger;
            _kafkaSetting = kafkaSetting.Value;
            _eventDispatcher = eventDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting utility network updated event consumer worker at: {time}", DateTimeOffset.Now);

            try
            {
                if (String.IsNullOrEmpty(_kafkaSetting.UtilityNetworkNotificationsTopic))
                    throw new ApplicationException("UtilityNetworkNotificationsTopic Kafka app setting must be set!");

                _kafkaConsumer = _eventDispatcher.Config("utility_network_updated_event_" + Guid.NewGuid(), c => {
                    var kafkaConfig = c.UseKafka(_kafkaSetting.Server);

                    if (_kafkaSetting.CertificateFilename != null)
                    {
                        kafkaConfig.WithCertificate(_kafkaSetting.CertificateFilename);
                    }
                })
              .Logging(l => l.UseSerilog())
              .Positions(x =>
              {
                  x.SetInitialPosition(StartFromPosition.Now);
                  x.StoreInMemory();
              })
              .Topics(t => t.Subscribe(_kafkaSetting.UtilityNetworkNotificationsTopic))
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
