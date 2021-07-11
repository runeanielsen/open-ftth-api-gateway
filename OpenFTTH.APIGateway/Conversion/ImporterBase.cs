using Npgsql;
using OpenFTTH.APIGateway.Settings;
using System;
using System.Data;

namespace OpenFTTH.APIGateway.Conversion
{
    public abstract class ImporterBase
    {
        protected GeoDatabaseSetting _geoDatabaseSetting;

        public ImporterBase(GeoDatabaseSetting geoDatabaseSetting)
        {
            _geoDatabaseSetting = geoDatabaseSetting;
        }

        protected void CreateTableLogColumn(string tableName)
        {
            var conn = GetConnection();

            var logCmd = conn.CreateCommand();

            // Add status column
            try
            {
                logCmd.CommandText = "ALTER TABLE " + tableName + " ADD COLUMN IF NOT EXISTS status varchar";
                logCmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }

            logCmd.Dispose();
        }

        protected void LogStatus(NpgsqlCommand cmd, string tableName, string statusText, string externalId)
        {
            cmd.CommandText = @"UPDATE " + tableName + " set status = @statusText where external_id ='" + externalId + "'";
            cmd.Parameters.AddWithValue("statusText", statusText);
            cmd.ExecuteNonQuery();
        }

        protected IDbConnection GetConnection()
        {
            var conn = new NpgsqlConnection(_geoDatabaseSetting.PostgresConnectionString);
            conn.Open();
            return conn;
        }
    }
}
