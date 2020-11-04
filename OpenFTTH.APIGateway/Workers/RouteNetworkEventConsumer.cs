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

                var offset = GetLatestOffset();

                _kafkaConsumer = _eventDispatcher.Config("route_network_event_" + Guid.NewGuid(), c => c.UseKafka(_kafkaSetting.Server))
                              //.Logging(l => l.UseSerilog())
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

        private long GetLatestOffset()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "route_network_event_get_offset_" + Guid.NewGuid(),
                BootstrapServers = _kafkaSetting.Server,
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Latest
            };
            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(_kafkaSetting.RouteNetworkEventTopic);

                var test = c.Consume();

                var position = c.Position(new TopicPartition(_kafkaSetting.RouteNetworkEventTopic, new Partition()));
                return position.Value;
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping background worker");
            _kafkaConsumer.Dispose();

            await Task.CompletedTask;
        }

    }
}
