using Microsoft.Extensions.Logging;
using Npgsql;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
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

        private ILogger<ConversionRunner> _logger;

        public ConversionRunner(ILoggerFactory loggerFactory, IEventStore eventSTore, GeoDatabaseSetting geoDatabaseSettings, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _loggerFactory = loggerFactory;
            _eventStore = eventSTore;
            _geoDatabaseSetting = geoDatabaseSettings;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;

            _logger = loggerFactory.CreateLogger<ConversionRunner>();
        }

        public void Run()
        {
            _logger.LogInformation("Checking conversion database...");

            if (CheckIfConversionSchemaExists())
            {
                /*
                var specResult = new CreateSpecifications(_commandDispatcher, _queryDispatcher).Run();

                if (specResult.IsFailed)
                {
                    _logger.LogInformation("Database already contain converted data. Will therefore not seed conversion data.");
                    return;
                }
                */

                //new SpanEquipmentImporter(_loggerFactory.CreateLogger<SpanEquipmentImporter>(), _eventStore, _geoDatabaseSetting, _commandDispatcher, _queryDispatcher).Run();

                new NodeContainerImporter(_loggerFactory.CreateLogger<NodeContainerImporter>(), _workTaskId, _eventStore, _geoDatabaseSetting, _commandDispatcher, _queryDispatcher).Run();
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
