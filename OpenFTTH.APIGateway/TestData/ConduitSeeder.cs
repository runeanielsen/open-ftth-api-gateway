﻿using FluentResults;
using Microsoft.Extensions.Options;
using Npgsql;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.TestData;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.TestData
{
    public class ConduitSeeder
    {
        private DatabaseSetting _databaseSetting;
        private ICommandDispatcher _commandDispatcher;


        public ConduitSeeder(DatabaseSetting databaseSetting, ICommandDispatcher commandDispatcher)
        {
            _databaseSetting = databaseSetting;
            _commandDispatcher = commandDispatcher;
        }

        public void Run()
        {
            Log.Information("Starting conduit seed...");
            var conduits = LoadConduitsFromConversionDatabase();
            AddConduitsToNetwork(conduits);

            Log.Information("Seeding of conduits finish!");
        }

        private List<ConduitForConversion> LoadConduitsFromConversionDatabase()
        {
            List<ConduitForConversion> conduitForConversions = new List<ConduitForConversion>();

            using var dbConn = GetConnection();
            dbConn.Open();

            using var dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = "SELECT * FROM conversion.bentley_multiconduit_conversion_result";

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
            foreach (var conduit in conduits)
            {
                if (conduit.ConduitSpec != null)
                {
                    PlaceConduit(conduit.ExternalId, conduit.ConduitSpec.SpecId, conduit.SegmentIds, conduit.ConduitSpec.AditionalSpecs, conduit.ConduitSpec.MarkingColor);
                }
            }
        }

        private void PlaceConduit(string externalId, Guid specificationId, List<Guid> segmentIds, List<Guid> additionalStructureSpecIds, string markingColor)
        {
            if (markingColor != null)
            {

            }

            RouteNetworkElementIdList walkIds = new RouteNetworkElementIdList();
            walkIds.AddRange(segmentIds);

            // Register walk of interest
            var walkOfInterestId = Guid.NewGuid();
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walkIds);
            var registerWalkOfInterestCommandResult = _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand).Result;

            if (registerWalkOfInterestCommandResult.IsFailed)
            {
                Log.Information("Failed to add conduit: " + externalId + " Error: " + registerWalkOfInterestCommandResult.Errors.First().Message);
                return;
            }


            // Place conduit
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), specificationId, registerWalkOfInterestCommandResult.Value)
            {
                MarkingInfo = markingColor != null ? new MarkingInfo() { MarkingColor = markingColor } : null
            };

            var placeSpanEquipmentResult = _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand).Result;

            if (placeSpanEquipmentResult.IsFailed)
            {
                Log.Information("Failed to add conduit: " + externalId + " Error: " + placeSpanEquipmentResult.Errors.First().Message);
                return;
            }

            // Place additional structures
            if (additionalStructureSpecIds.Count > 0)
            {
                var addStructure = new PlaceAdditionalStructuresInSpanEquipment(
                 spanEquipmentId: placeSpanEquipmentCommand.SpanEquipmentId,
                 structureSpecificationIds: additionalStructureSpecIds.ToArray()
                );

                var addStructureResult = _commandDispatcher.HandleAsync<PlaceAdditionalStructuresInSpanEquipment, Result>(addStructure).Result;

                if (addStructureResult.IsFailed)
                {
                    Log.Information("Failed to add additional structures to: " + externalId + " Error: " + placeSpanEquipmentResult.Errors.First().Message);
                    return;
                }
            }
        }

        private IDbConnection GetConnection()
        {
            return new NpgsqlConnection($"Host={_databaseSetting.Host};Port={_databaseSetting.Port};Username={_databaseSetting.Username};Password={_databaseSetting.Password};Database={_databaseSetting.Database}");
        }

        private class ConduitForConversion
        {
            public string ExternalId { get; set; }
            public string ExternalSpec { get; set; }
            public List<Guid> SegmentIds = new List<Guid>();

            public ConduitSpecInfo ConduitSpec { get; set;}

            public ConduitForConversion(string externalId, string externalSpec, string segmentIds)
            {
                ExternalId = externalId;
                ExternalSpec = externalSpec;

                var segmentSplit = segmentIds.Split(',');

                foreach (var segmentId in segmentSplit)
                {
                    SegmentIds.Add(Guid.Parse(segmentId));
                }

                ConduitSpec = MapToSpanEquipmentSpecification(externalSpec);
            }

            private ConduitSpecInfo MapToSpanEquipmentSpecification(string externalSpec)
            {

                if (externalSpec.StartsWith("40-5x10"))
                {
                    // TODO: handle special editing
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_5x10, GetMarkingText(externalSpec));
                }

                if (externalSpec.StartsWith("40-6x10"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_6x10, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("50-5x10"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø40_5x10, GetMarkingText(externalSpec));

                if (externalSpec.StartsWith("50-10x10"))
                    return new ConduitSpecInfo(TestSpecifications.Multi_Ø50_10x10, GetMarkingText(externalSpec));

                if (externalSpec == "40 mm tomt rør")
                    return new ConduitSpecInfo(TestSpecifications.Tomrør_Ø40_Orange);


                // flexrør
                if (externalSpec.StartsWith("flexrør") || externalSpec == "oe 40 mm tomt rør")
                {
                    return new ConduitSpecInfo(TestSpecifications.Flex_Ø40_Red, GetAdditionalStructures(externalSpec));
                }

                // Ø110 r'r
                if (externalSpec.StartsWith("rør 110"))
                {
                    return new ConduitSpecInfo(TestSpecifications.Tomrør_Ø110_Red, GetAdditionalStructures(externalSpec));
                }


                Log.Debug($"Don't know how to handle spec: {externalSpec}");

                return null;
            }

            private string GetMarkingText(string spec)
            {
                if (spec.Contains("blå tape"))
                    return "Blå tape";

                if (spec.Contains("rød tape"))
                    return "Rød tape";

                if (spec.Contains("grø tape"))
                    return "Grøn tape";

                if (spec.Contains("gul tape"))
                    return "Gul tape";

                if (spec.Contains("hvid tape"))
                    return "Hvid tape";

                if (spec.Contains("sort tape"))
                    return "Sort tape";

                return null;
            }

            private List<Guid> GetAdditionalStructures(string spec)
            {
                List<Guid> additionalStructureSpecs = new List<Guid>();

                if (spec.Contains("1bk"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Black);

                if (spec.Contains("1bl"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);

                if (spec.Contains("1sl"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Silver);

                if (spec.Contains("1wh"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_White);

                if (spec.Contains("1ye"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);

                if (spec.Contains("1br"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Brown);

                if (spec.Contains("1gr"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Green);

                if (spec.Contains("1or"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Orange);

                if (spec.Contains("1rd"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Red);

                if (spec.Contains("1ro"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Pink);

                if (spec.Contains("1tu"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Turquoise);

                if (spec.Contains("1vi"))
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Violet);

                if (spec.Contains("2bl"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);
                }

                if (spec.Contains("2ye"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                }

                if (spec.Contains("bl+ye+bk+wh"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Black);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_White);
                }

                // Ø110

                if (spec.Contains("110-4x10-red"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_White);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Green);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Red);
                }


                if (spec.Contains("110-5x10"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_White);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Green);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Red);
                }

                if (spec.Contains("110-6x10"))
                {
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Blue);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Yellow);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_White);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Green);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Red);
                    additionalStructureSpecs.Add(TestSpecifications.Ø10_Black);
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
