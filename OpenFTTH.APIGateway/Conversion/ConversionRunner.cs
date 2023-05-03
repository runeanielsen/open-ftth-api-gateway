using Microsoft.Extensions.Logging;
using Npgsql;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.RouteNetwork.Business.RouteElements.StateHandling;
using OpenFTTH.TestData;
using System;
using System.Data;

namespace OpenFTTH.APIGateway.Conversion
{
    public class ConversionRunner
    {
        private static Guid _workTaskId = Guid.Parse("299c3e6f-c764-4566-81ab-3e9413aa4fca");

        private ILoggerFactory _loggerFactory;
        private IEventStore _eventStore;
        private GeoDatabaseSetting _geoDatabaseSetting;
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;
        private IRouteNetworkState _routeNetworkState;

        private ILogger<ConversionRunner> _logger;

        public ConversionRunner(ILoggerFactory loggerFactory, IEventStore eventSTore, GeoDatabaseSetting geoDatabaseSettings, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IRouteNetworkState routeNetworkState)
        {
            _loggerFactory = loggerFactory;
            _eventStore = eventSTore;
            _geoDatabaseSetting = geoDatabaseSettings;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _routeNetworkState = routeNetworkState;

            _logger = loggerFactory.CreateLogger<ConversionRunner>();
        }

        public void Run()
        {
            _logger.LogInformation("Checking if conversion schema exists...");


            if (CheckIfConversionSchemaExists())
            {
                _logger.LogInformation("Start seeding database with test specifications...");
                 var result = new TestSpecifications(_loggerFactory, _commandDispatcher, _queryDispatcher, _eventStore).Run();
                _logger.LogInformation("Finish seeding database with test specifications.");

                


                var localDb = new Settings.GeoDatabaseSetting()
                {
                    Host = "localhost",
                    Database = "open-ftth",
                    Username = "postgres",
                    Password = "postgres",
                    Port = "5432"
                };

                var dbToReadConversionDataFrom = _geoDatabaseSetting;

                //new Checker(_loggerFactory.CreateLogger<Checker>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

               

                new CableSpanEquipmentImporter(_loggerFactory.CreateLogger<ConduitSpanEquipmentImporter>(), _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher, _routeNetworkState).Run();

                new NodeContainerImporter(_loggerFactory.CreateLogger<NodeContainerImporter>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

                new RackImporter(_loggerFactory.CreateLogger<RackImporter>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

                new TerminalEquipmentImporter(_loggerFactory.CreateLogger<TerminalEquipmentImporter>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

                new TerminalStructureImporter(_loggerFactory.CreateLogger<TerminalStructureImporter>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

                new SegmentToTerminalConnectionsImporter(_loggerFactory.CreateLogger<SegmentToTerminalConnectionsImporter>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

                new TerminalToTerminalConnectionsImporter(_loggerFactory.CreateLogger<TerminalToTerminalConnectionsImporter>(), _workTaskId, _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher).Run();

                new CablePlaceInConduit(_loggerFactory.CreateLogger<ConduitSpanEquipmentImporter>(), _eventStore, dbToReadConversionDataFrom, _commandDispatcher, _queryDispatcher, _routeNetworkState).Run();

                

            }
            else
            {
                _logger.LogInformation("No conversion schema exists. Will therefore not seed conversion data.");
            }
        }

        private bool CheckIfConversionSchemaExists()
        {
            using var dbConn = GetConnection();

            using var dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = "SELECT schema_name FROM information_schema.schemata WHERE schema_name = 'conversion'";

            using var dbReader = dbCmd.ExecuteReader();

            if (dbReader.Read())
                return true;
            else
                return false;
        }

        

        private IDbConnection GetConnection()
        {
            var conn = new NpgsqlConnection(_geoDatabaseSetting.PostgresConnectionString);
            conn.Open();
            return conn;
        }
    }
}
