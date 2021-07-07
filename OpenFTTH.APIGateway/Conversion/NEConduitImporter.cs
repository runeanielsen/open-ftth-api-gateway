using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.CQRS;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.EventSourcing;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.TestData;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.Graph;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace OpenFTTH.APIGateway.Conversion
{
    public class NEConduitImporter
    {
        private static Guid _neMultiConduitConversion = Guid.Parse("299c3e6f-c764-4566-81ab-3e9413aa4fca");

        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;
        private ILogger<NEConduitImporter> _logger;
        private IEventStore _eventStore;
        private GeoDatabaseSetting _geoDatabaseSetting;

        private string _tableName = "conversion.ne_multiconduit_conversion_result";

        public NEConduitImporter(ILogger<NEConduitImporter> logger, IEventStore eventSTore, GeoDatabaseSetting geoDatabaseSettings, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _logger = logger;
            _eventStore = eventSTore;
            _geoDatabaseSetting = geoDatabaseSettings;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public void Run()
        {
            if (_geoDatabaseSetting != null)
            {
                _logger.LogInformation("Checking conversion database...");

                var specResult = new NESpecifications(_commandDispatcher, _queryDispatcher).Run();

                if (specResult.IsFailed)
                {
                    _logger.LogInformation("Database already contain converted data. Will therefore not seed conversion data.");
                    return;
                }

                _logger.LogInformation("Starting conversion...");


                CreateTableLogColumn(_tableName);

                var conduits = LoadConduitsFromConversionDatabase(_tableName);
                

                AddConduitsToNetwork(conduits);

                _logger.LogInformation("Seeding of conduits finish!");
            }
            else
            {
                _logger.LogInformation("No conversion_database_connection_string env variable specified. Will not seed database with conversion data");
            }

            
        }

        private List<ConduitForConversion> LoadConduitsFromConversionDatabase(string tableName)
        {
            List<ConduitForConversion> conduitForConversions = new List<ConduitForConversion>();

            using var dbConn = GetConnection();

            using var dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = "SELECT * FROM " + tableName;

            using var dbReader = dbCmd.ExecuteReader();

            while (dbReader.Read())
            {
                var external_id = dbReader.GetString(1).Trim();
                var external_spec = dbReader.GetString(2).Trim().ToLower();
                var segments_ids = dbReader.GetString(3);

                var conduit = new ConduitForConversion(external_id, external_spec, segments_ids);

                conduitForConversions.Add(conduit);
            }

            dbConn.Close();

            return conduitForConversions;
        }

        private void AddConduitsToNetwork(List<ConduitForConversion> conduits)
        {
            using var conn = GetConnection();

            var logCmd = conn.CreateCommand();

            var trans = conn.BeginTransaction();
            logCmd.Transaction = trans;

            foreach (var conduit in conduits)
            {
                if (conduit.ConduitSpec != null)
                {
                    var result = PlaceConduit(conduit.ExternalId, conduit.ConduitSpec.SpecId, conduit.SegmentIds, conduit.ConduitSpec.AditionalSpecs, conduit.ConduitSpec.MarkingColor);

                    if (result.IsFailed)
                    {
                        LogStatus((NpgsqlCommand)logCmd, _tableName, result.Reasons.First().Message, conduit.ExternalId);
                    }
                }
            }

            trans.Commit();
        }

        private Result PlaceConduit(string externalId, Guid specificationId, List<Guid> segmentIds, List<Guid> additionalStructureSpecIds, string markingColor)
        {
            Guid correlationId = Guid.NewGuid();

            RouteNetworkElementIdList walkIds = new RouteNetworkElementIdList();
            walkIds.AddRange(segmentIds);

            // Register walk of interest
            var walkOfInterestId = Guid.NewGuid();
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(correlationId, new UserContext("conversion", _neMultiConduitConversion), walkOfInterestId, walkIds);

            var registerWalkOfInterestCommandResult = _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand).Result;

            if (registerWalkOfInterestCommandResult.IsFailed)
            {
                _logger.LogInformation("Failed to add conduit: " + externalId + " Error: " + registerWalkOfInterestCommandResult.Errors.First().Message);
                return registerWalkOfInterestCommandResult;
            }

            // conduit name
            var nextConduitSeqStr = _eventStore.Sequences.GetNextVal("conduit").ToString();

            var conduitName = "R" + nextConduitSeqStr.PadLeft(6, '0');
            var namingInfo = new NamingInfo(conduitName, null);

            // Place conduit
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(correlationId, new UserContext("conversion", _neMultiConduitConversion), Guid.NewGuid(), specificationId, registerWalkOfInterestCommandResult.Value)
            {
                MarkingInfo = markingColor != null ? new MarkingInfo() { MarkingColor = markingColor } : null,
                NamingInfo = namingInfo
            };

            var placeSpanEquipmentResult = _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand).Result;

            if (placeSpanEquipmentResult.IsFailed)
            {
                _logger.LogInformation("Failed to add conduit: " + externalId + " Error: " + placeSpanEquipmentResult.Errors.First().Message);
                return placeSpanEquipmentResult;
            }

            // Place additional structures
            if (additionalStructureSpecIds.Count > 0)
            {
                var addStructure = new PlaceAdditionalStructuresInSpanEquipment(correlationId, new UserContext("conversion", _neMultiConduitConversion),
                 spanEquipmentId: placeSpanEquipmentCommand.SpanEquipmentId,
                 structureSpecificationIds: additionalStructureSpecIds.ToArray()
                );

                var addStructureResult = _commandDispatcher.HandleAsync<PlaceAdditionalStructuresInSpanEquipment, Result>(addStructure).Result;

                if (addStructureResult.IsFailed)
                {
                    _logger.LogInformation("Failed to add additional structures to: " + externalId + " Error: " + placeSpanEquipmentResult.Errors.First().Message);
                    return addStructureResult;
                }
            }

            return Result.Ok();
        }


        private void CreateTableLogColumn(string tableName)
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


            var cmd = conn.CreateCommand();

            // make sure it is null
            cmd.CommandText = "update " + tableName + " set status = null";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
        }

        private void LogStatus(NpgsqlCommand cmd, string tableName, string statusText, string externalId)
        {
            cmd.CommandText = @"UPDATE " + tableName + " set status = @statusText where external_id ='" + externalId + "'";
            cmd.Parameters.AddWithValue("statusText", statusText);
            cmd.ExecuteNonQuery();
        }

        private IDbConnection GetConnection()
        {
            var conn = new NpgsqlConnection(_geoDatabaseSetting.PostgresConnectionString);
            conn.Open();
            return conn;
        }

        private class ConduitForConversion
        {
            public string ExternalId { get; set; }
            public string ExternalSpec { get; set; }

            public List<Guid> SegmentIds = new List<Guid>();
            
            public ConduitSpecInfo ConduitSpec { get; set;}

            public bool MissingSegments = false;

            public ConduitForConversion(string externalId, string externalSpec, string segmentIds)
            {
                ExternalId = externalId;
                ExternalSpec = externalSpec;

                var segmentSplit = segmentIds.Split(',');

                foreach (var segmentId in segmentSplit)
                {
                    if (segmentId.Length < 32)
                    {
                        MissingSegments = true;
                    }
                    else
                    {
                        SegmentIds.Add(Guid.Parse(segmentId));
                    }
                }

                ConduitSpec = MapToSpanEquipmentSpecification(externalSpec);
            }

            private ConduitSpecInfo MapToSpanEquipmentSpecification(string externalSpec)
            {
                // Ø 12
                if (externalSpec.StartsWith("12mm"))
                    return new ConduitSpecInfo(TestSpecifications.CustomerConduit_Ø12_Orange, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("ø12"))
                    return new ConduitSpecInfo(TestSpecifications.CustomerConduit_Ø12_Orange, GetMarkingText(externalSpec));

                // Ø 32
                if (externalSpec.StartsWith("ø32"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø32_3x10, GetMarkingText(externalSpec));

                // Ø40 flexrør
                if (externalSpec.StartsWith("flx"))
                    return new ConduitSpecInfo(TestSpecifications.Flex_Ø40_Red, GetAdditionalStructures(externalSpec));

                if (externalSpec.StartsWith("flexrør"))
                    return new ConduitSpecInfo(TestSpecifications.Flex_Ø40_Red, GetAdditionalStructures(externalSpec));

                if (externalSpec == "40-2x10-grøn-blå")
                    return new ConduitSpecInfo(TestSpecifications.Flex_Ø40_Red, GetAdditionalStructures(externalSpec));

                if (externalSpec == "40-3x10")
                    return new ConduitSpecInfo(TestSpecifications.Flex_Ø40_Red, GetMarkingText(externalSpec));

                // Ø40 tomrør
                if (externalSpec == "ø40 tomrør")
                    return new ConduitSpecInfo(TestSpecifications.Tomrør_Ø40_Orange, GetMarkingText(externalSpec));

                if (externalSpec == "ø40 dantex")
                    return new ConduitSpecInfo(TestSpecifications.Tomrør_Ø40_Orange, GetMarkingText(externalSpec));

                if (externalSpec == "ø40 4x10")
                    return new ConduitSpecInfo(TestSpecifications.Tomrør_Ø40_Orange, GetMarkingText(externalSpec));

                // Ø40 multi
                if (externalSpec == "ø40 5x10" || externalSpec == "ø40 5x10 dantex")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_5x10, GetMarkingText(externalSpec));

                if (externalSpec == "ø40 5x10 rød")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_5x10_Red, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("ø40+6x10"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_6x10, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("ø40 12x7"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_12x7, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("ø40+12x7"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_12x7, GetMarkingText(externalSpec));

                // Ø50 tomrør
                if (externalSpec == "ø50 dantex")
                    return new ConduitSpecInfo(TestSpecifications.Tomrør_Ø50_Orange);

                // Ø50
                if (externalSpec ==  "ø50 5x10")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_5x10, GetMarkingText(externalSpec));

                if (externalSpec == "ø50 5x10+12x7 color")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø50_12x7_5x10, GetMarkingText(externalSpec));

                if (externalSpec == "ø50+5x10+12x7dantex")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø50_12x7_5x10, GetMarkingText(externalSpec));

                if (externalSpec == "ø50 5x10+12x7")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø50_12x7_5x10_BlueYellow, GetMarkingText(externalSpec));

                if (externalSpec == "ø50 5x10+12x7 hvigrø")
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø50_12x7_5x10_GreenWhite, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("ø50+10x10") || externalSpec.StartsWith("ø50 10x10"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø50_10x10, GetMarkingText(externalSpec));


                Log.Warning($"Don't know how to handle spec: '{externalSpec}'");

                return null;
            }

            private string GetMarkingText(string spec)
            {
                if (spec.Contains("core"))
                    return "Sort streg";

                return null;
            }

            private List<Guid> GetAdditionalStructures(string spec)
            {
                List<Guid> additionalStructureSpecs = new List<Guid>();

                if (spec.Contains("1x10 sort"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Black);

                if (spec.Contains("1x10 blå"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);

                if (spec.Contains("1x10 grå"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Silver);

                if (spec.Contains("1x10 hvid"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_White);

                if (spec.Contains("1x10 gul"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);

                if (spec.Contains("1x10 brun"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Brown);

                if (spec.Contains("1x10 grøn"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Green);

                if (spec.Contains("1x10 orange"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Orange);

                if (spec.Contains("1x10 rød"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Red);

                if (spec.Contains("1x10 pink"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Pink);

                if (spec.Contains("1x10 turkis"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Turquoise);

                if (spec.Contains("1x10 violet"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Violet);

                if (spec.Contains("blågul"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                }

                return additionalStructureSpecs;
            }

            public override string ToString()
            {
                string result = ExternalSpec + ": ";
                if (ConduitSpec != null)
                {
                    result += "MARKING: " + ConduitSpec.MarkingColor + " ADDS: " + ConduitSpec.AditionalSpecs.Count;

                }
                else
                {
                    result += " NO SPEC MAPPING";
                }

                return result;
            }

        }

        private class ConduitSpecInfo
        {
            public Guid SpecId { get; set; }
            public string MarkingColor { get; set; }

            public List<Guid> AditionalSpecs = new List<Guid>();

            public ConduitSpecInfo(Guid specId, string markingColor = null)
            {
                SpecId = specId;
                MarkingColor = markingColor;
            }

            public ConduitSpecInfo(Guid specId, List<Guid> aditionalSpecs)
            {
                SpecId = specId;
                AditionalSpecs = aditionalSpecs;
            }
        }
    }
}
