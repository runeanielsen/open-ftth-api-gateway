using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenFTTH.APIGateway.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenFTTH.APIGateway.DynamicProperties
{
    public class DynamicPropertiesClient
    {
        private readonly string _schemaName = "web";
        private readonly string _installationTableName = "view_installation";

        private readonly ILogger<DynamicPropertiesClient> _logger;
        private readonly GeoDatabaseSetting _geoDatabaseSetting;

        private bool _isInitialized = false;
        private bool _installationTableOrViewExists = false;

        public DynamicPropertiesClient(ILogger<DynamicPropertiesClient> logger, IOptions<GeoDatabaseSetting> geoDatabaseSetting)
        {
            _logger = logger;
            _geoDatabaseSetting = geoDatabaseSetting.Value;
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;

            if (CheckIfTableExists(_schemaName, _installationTableName) || CheckIfViewExists(_schemaName, _installationTableName))
            {
                _installationTableOrViewExists = true;
            }

            _isInitialized = true;
        }

        public List<DynamicPropertiesSection> ReadProperties(Guid equipment_id)
        {
            // We lazy initialize when properties are read the first time
            Initialize();

            // If there's not an installation table in the database, we just return an empty property section list
            if (!_installationTableOrViewExists)
                return new List<DynamicPropertiesSection> { };

            using var dbConn = GetConnection();

            using var selectCmd = dbConn.CreateCommand();

            selectCmd.CommandText = $"SELECT * from {_schemaName}.{_installationTableName} WHERE equipment_id = '{equipment_id}'";

            using var installaionReader = selectCmd.ExecuteReader();

            // Dictionary used to lookup sections by name
            Dictionary<string, DynamicPropertiesSection> sectionByName = new();
          
            if (installaionReader.Read())
            {
                for (int fieldIndex = 0; fieldIndex < installaionReader.FieldCount; fieldIndex++)
                {
                    var tablefieldName = installaionReader.GetName(fieldIndex);

                    // Skip if id field
                    if (tablefieldName.ToLower() == "equipment_id")
                        continue;

                    // Check if underscore is present
                    if (!tablefieldName.Contains("_"))
                    {
                        _logger.LogWarning($"Invalid field name: '{tablefieldName}' in installation table. Must contain one underscore to seperate section name from field name.");
                        continue;
                    }

                    var fieldNameSplit = tablefieldName.Split('_');

                    var sectionName = fieldNameSplit[0];
                    var propertyName = fieldNameSplit[1];
                    var propertyValue = installaionReader.IsDBNull(fieldIndex) ? null : installaionReader.GetValue(fieldIndex).ToString();

                    // Make sure property section exists
                    if (!sectionByName.ContainsKey(sectionName))
                    {
                        sectionByName.Add(sectionName, new DynamicPropertiesSection(sectionName));
                    }

                    // Add property
                    sectionByName[sectionName].Properties.Add(new DynamicProperty() { Name = propertyName, Value = propertyValue });   
                }

                return sectionByName.Values.ToList();
            }
            else
            {
                _logger.LogWarning($"Cannot find any installation with id: '{equipment_id.ToString()}'");
                return new List<DynamicPropertiesSection> { };
            }
        }

        protected bool CheckIfTableExists(string schemaName, string tableName)
        {
            using var dbConn = GetConnection();

            using var dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = $"SELECT tablename FROM pg_tables WHERE schemaname = '{schemaName}' AND tablename = '{tableName.Split('.').Last()}';";

            using var dbReader = dbCmd.ExecuteReader();

            if (dbReader.Read())
                return true;
            else
                return false;
        }

        protected bool CheckIfViewExists(string schemaName, string viewName)
        {
            using var dbConn = GetConnection();

            using var dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = $"SELECT viewname FROM pg_views WHERE schemaname = '{schemaName}' AND viewname = '{viewName.Split('.').Last()}';";

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
