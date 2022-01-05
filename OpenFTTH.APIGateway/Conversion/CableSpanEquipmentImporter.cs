using FluentResults;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.CQRS;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.EventSourcing;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.TestData;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Tracing;
using OpenFTTH.UtilityGraphService.Business.Graph;
using OpenFTTH.UtilityGraphService.Business.NodeContainers.Projections;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments.Projections;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments.QueryHandlers.Trace;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace OpenFTTH.APIGateway.Conversion
{
    public class CableSpanEquipmentImporter : ImporterBase
    {
        private static Guid _workTaskId = Guid.Parse("57859b8e-f033-4d3c-8226-88ae162a166e");

        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;
        private ILogger<ConduitSpanEquipmentImporter> _logger;
        private IEventStore _eventStore;
        private UtilityNetworkProjection _utilityNetwork;
        private LookupCollection<SpanEquipmentSpecification> _spanEquipmentSpecifications;

        //private string _tableName = "conversion.ne_multiconduit_conversion_result";
        private string _tableName = "conversion.fibercables";

        private string _directInRouteTableName = "conversion.cable_in_route_segment_rels";

        public CableSpanEquipmentImporter(ILogger<ConduitSpanEquipmentImporter> logger, IEventStore eventSTore, GeoDatabaseSetting geoDatabaseSettings, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : base(geoDatabaseSettings)
        {
            _logger = logger;
            _eventStore = eventSTore;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _utilityNetwork = _eventStore.Projections.Get<UtilityNetworkProjection>();
            _spanEquipmentSpecifications = _eventStore.Projections.Get<SpanEquipmentSpecificationsProjection>().Specifications;
        }

        public void Run()
        {

            _logger.LogInformation("Starting span equipment cable conversion...");

            CreateTableLogColumn(_tableName);

            var conduits = LoadSpanEquipmentsFromConversionDatabase(_tableName);


            AddCablesToNetwork(conduits);

            _logger.LogInformation("Conversion of span equipment cables finish!");
        }

        private List<SpanEquipmentForConversion> LoadSpanEquipmentsFromConversionDatabase(string tableName)
        {
            List<SpanEquipmentForConversion> conduitForConversions = new List<SpanEquipmentForConversion>();

            using var dbConn = GetConnection();

            using var dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = "SELECT external_id, span_equipment_id, span_equipment_spec_name, segment_ids, parent_span_equipment_ids, status FROM " + tableName + " WHERE status is null ORDER BY external_id";

            using var dbReader = dbCmd.ExecuteReader();

            while (dbReader.Read())
            {
                var externalId = dbReader.GetString(0).Trim();
                var spanSegmentId = Guid.Parse(dbReader.GetString(1));
                var externalSpec = dbReader.GetString(2).Trim().ToLower();
                var routeSegmentsIds = dbReader.GetString(3);
                var parentConduits = dbReader.GetString(4);

                var conduit = new SpanEquipmentForConversion(spanSegmentId, externalId, externalSpec, routeSegmentsIds, parentConduits);

                conduitForConversions.Add(conduit);
            }

            dbConn.Close();

            return conduitForConversions;
        }

        private void AddCablesToNetwork(List<SpanEquipmentForConversion> spanEquipments)
        {
            using var conn = GetConnection();

            PrepareTables(conn);

            using var logCmd = conn.CreateCommand() as NpgsqlCommand;

            foreach (var spanEquipment in spanEquipments)
            {
                
                var result = PlaceCableSpanEquipment(logCmd, spanEquipment.Id, spanEquipment.ExternalId, spanEquipment.SpecificatonId, spanEquipment.SegmentIds, spanEquipment.ConduitRels);

                if (result.IsFailed)
                {
                    LogStatus((NpgsqlCommand)logCmd, _tableName, result.Errors.First().Message, spanEquipment.ExternalId);
                }
                else
                {
                    LogStatus((NpgsqlCommand)logCmd, _tableName, "OK", spanEquipment.ExternalId);
                }
            }
        }

        public Result PlaceCableSpanEquipment(NpgsqlCommand logCmd, Guid spanEquipmentId, string externalId, Guid specificationId, List<Guid> routeSegmentIds, List<CableConduitRel> conduitRels)
        {
            Guid correlationId = Guid.NewGuid();

            RouteNetworkElementIdList walkIds = new RouteNetworkElementIdList();
            walkIds.AddRange(routeSegmentIds);

            // Cable name
            var nextConduitSeqStr = _eventStore.Sequences.GetNextVal("cable").ToString();
            var conduitName = "K" + nextConduitSeqStr.PadLeft(6, '0');
            var namingInfo = new NamingInfo(conduitName, null);


            // Get validated walk of interest
            var walk = new RouteNetworkElementIdList();
            walk.AddRange(routeSegmentIds);

            var validateInterestCommand = new ValidateWalkOfInterest(correlationId, new UserContext("conversion", _workTaskId), walk);

            var validateInterestResult = _commandDispatcher.HandleAsync<ValidateWalkOfInterest, Result<ValidatedRouteNetworkWalk>>(validateInterestCommand).Result;

            if (validateInterestResult.IsFailed)
                return Result.Fail(validateInterestResult.Errors.First());
       
            // trace all conduits
            var conduitsTraceResult = TraceAllConduits(conduitRels);

            var routingHops = BuildRouteHops(validateInterestResult.Value, conduitsTraceResult, externalId);


            // Place cable
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInUtilityNetwork(correlationId, new UserContext("conversion", _workTaskId), spanEquipmentId, specificationId, routingHops.ToArray())
            {
                NamingInfo = namingInfo,
            };

            
            var placeSpanEquipmentResult = _commandDispatcher.HandleAsync<PlaceSpanEquipmentInUtilityNetwork, Result>(placeSpanEquipmentCommand).Result;

            if (placeSpanEquipmentResult.IsFailed)
            {
                _logger.LogInformation("Failed to add cable: " + externalId + " Error: " + placeSpanEquipmentResult.Errors.First().Message);
                LogStatus((NpgsqlCommand)logCmd, _tableName, placeSpanEquipmentResult.Errors.First().Message, externalId);
                return placeSpanEquipmentResult;
            }
            
            return Result.Ok();
        }

        private List<RoutingHop> BuildRouteHops(ValidatedRouteNetworkWalk cableWalk, List<ConduitTraceResult> conduitsTraceResult, string externalId)
        {
            List<RoutingHop> result = new();

            // route nodes hash list for quick lookup
            var shortestPathWalkNodeIdHash = cableWalk.NodeIds.ToHashSet<Guid>();

            List<Guid> directlyInRouteNetworkWalk = new();

            var nodeIds = cableWalk.NodeIds;
            var segmentIds = cableWalk.SegmentIds;

            HashSet<Guid> alreadyUsedSpanSegmentIds = new();

            Guid conduitToNodeId = Guid.Empty;

            for (int nodeIndex = 0; nodeIndex < nodeIds.Count; nodeIndex++)
            {
                var shortestPathTraceNodeId = nodeIds[nodeIndex];

                var nodeIdToFindTraceFrom = conduitToNodeId != Guid.Empty ? conduitToNodeId : shortestPathTraceNodeId;

                var routeNetworkTrace = FindConduitTraceThatStartOrEndInNode(conduitsTraceResult, shortestPathWalkNodeIdHash, nodeIdToFindTraceFrom, alreadyUsedSpanSegmentIds);

                conduitToNodeId = Guid.Empty;

                if (routeNetworkTrace != null)
                {
                    // If the conduit end on a a node in the shortest path trace we're lucky and happy
                    if (nodeIds.IndexOf(routeNetworkTrace.ToNodeId) > nodeIndex)
                    {
                        // First add eventually directly route network hop
                        if (directlyInRouteNetworkWalk.Count > 0)
                        {
                            directlyInRouteNetworkWalk.Add(segmentIds[nodeIndex - 1]);
                            directlyInRouteNetworkWalk.Add(shortestPathTraceNodeId);

                            result.Add(CreateRouteNetworkHop(directlyInRouteNetworkWalk, externalId));
                            directlyInRouteNetworkWalk.Clear();
                        }

                        result.Add(new RoutingHop(nodeIdToFindTraceFrom, routeNetworkTrace.SpanSegmentId));

                        // That's evil. We  forward the index in the loop to point to the end of trace. But it kind of feels good, like the good old C days :)
                        nodeIndex = nodeIds.IndexOf(routeNetworkTrace.ToNodeId) - 1;

                        // If we hit last node break
                        if (routeNetworkTrace.ToNodeId == cableWalk.ToNodeId)
                            break;
                    }
                    // If the conduit ends outside the shortest path trace, we set the next node to the trace end node, and hope a new related conduit is found from there
                    else
                    {
                        result.Add(new RoutingHop(nodeIdToFindTraceFrom, routeNetworkTrace.SpanSegmentId));

                        // We need to rely on the conduit to node, and hope there's a new conduit trace going from there
                        conduitToNodeId = routeNetworkTrace.ToNodeId;

                        // If we hit last node break
                        if (routeNetworkTrace.ToNodeId == cableWalk.ToNodeId)
                            break;
                    }

                }
                else
                {
                    if (nodeIndex == 0)
                    {
                        directlyInRouteNetworkWalk.Add(shortestPathTraceNodeId);
                    }
                    else
                    {
                        // The node
                        directlyInRouteNetworkWalk.Add(shortestPathTraceNodeId);

                        // Segment after node
                        if (segmentIds.Count > nodeIndex)
                            directlyInRouteNetworkWalk.Add(segmentIds[nodeIndex]);
                    }
                }
            }

            // First add eventually directly route network hop in the end of the cable route
            if (directlyInRouteNetworkWalk.Count > 0)
            {
                result.Add(CreateRouteNetworkHop(directlyInRouteNetworkWalk, externalId));
                directlyInRouteNetworkWalk.Clear();
            }

            return result;
        }

        private RoutingHop CreateRouteNetworkHop(List<Guid> directlyInRouteNetworkWalk, string externalId)
        {
            if (directlyInRouteNetworkWalk.Count > 2)
            {
                for (int i = 1; i < directlyInRouteNetworkWalk.Count; i+= 2)
                {
                    LogCableToRouteNetworkRelations(externalId, directlyInRouteNetworkWalk[i]);
                }
            }

            return new RoutingHop(directlyInRouteNetworkWalk.ToArray());
        }

        private void LogCableToRouteNetworkRelations(string externalId, Guid routeSegmentId)
        {
            if (_geoDatabaseSetting.Database != null)
            {
                using var conn = GetConnection();

                using var cmd = conn.CreateCommand();

                cmd.CommandText = @"INSERT INTO " + _directInRouteTableName + "(external_id, route_segment_id) VALUES(";

                cmd.CommandText += "'" + externalId + "',";
                cmd.CommandText += "'" + routeSegmentId.ToString() + "');";
                cmd.ExecuteNonQuery();
            }
        }

        private ConduitTraceResult FindConduitTraceThatStartOrEndInNode(List<ConduitTraceResult> conduitsTraceResult, HashSet<Guid> shortestPathNodeIdHash, Guid nodeId, HashSet<Guid> alreadyUsedSpanSegmentIds)
        {
            foreach (var traceResult in conduitsTraceResult)
            {
                if (!alreadyUsedSpanSegmentIds.Contains(traceResult.SpanSegmentId))
                {
                    // If the conduit don't start and end on nodes of the original cable route, then we can't use it
                    // now this check prevented not following conduit connection outside the shortest path trace
                    //if (nodeIdHash.Contains(traceResult.OriginalTrace.FromRouteNodeId) && nodeIdHash.Contains(traceResult.OriginalTrace.ToRouteNodeId))
                    //{
                    if (traceResult.OriginalTrace.FromRouteNodeId == nodeId)
                    {
                        traceResult.FromNodeId = traceResult.OriginalTrace.FromRouteNodeId;
                        traceResult.ToNodeId = traceResult.OriginalTrace.ToRouteNodeId;
                        
                        foreach (var spanSegmentId in traceResult.SpanSegmentIds)
                            alreadyUsedSpanSegmentIds.Add(spanSegmentId);

                        return traceResult;
                    }
                    else if (traceResult.OriginalTrace.ToRouteNodeId == nodeId)
                    {
                        traceResult.FromNodeId = traceResult.OriginalTrace.ToRouteNodeId;
                        traceResult.ToNodeId = traceResult.OriginalTrace.FromRouteNodeId;

                        foreach (var spanSegmentId in traceResult.SpanSegmentIds)
                            alreadyUsedSpanSegmentIds.Add(spanSegmentId);


                        return traceResult;
                    }
                }
            }

            return null;
        }

        private List<ConduitTraceResult> TraceAllConduits(List<CableConduitRel> conduitRels)
        {
            var result = new List<ConduitTraceResult>();

            HashSet<Guid> alreadyUsedSpanSegmentIds = new();

            foreach (var conduitRel in conduitRels)
            {
                if (_utilityNetwork.TryGetEquipment<SpanEquipment>(conduitRel.SpanEquipmentId, out var conduit))
                {
                    var spec = _spanEquipmentSpecifications[conduit.SpecificationId];

                    // single conduit
                    if (conduit.SpanStructures.Length == conduitRel.InnerConduitNumber && conduit.SpanStructures.Length == 1)
                    {
                        // we only have one conduit (the outer)
                        var spanSegmentIdToTrace = conduit.SpanStructures[0].SpanSegments[0].Id;

                        var traceBuilder = new RouteNetworkTraceResultBuilder(_queryDispatcher, _utilityNetwork);

                        var traceInfo = traceBuilder.GetTraceInfo(new List<SpanEquipment> { conduit }, spanSegmentIdToTrace);

                        if (traceInfo != null || traceInfo.RouteNetworkTraces.Count == 1)
                        {
                            if (!alreadyUsedSpanSegmentIds.Contains(spanSegmentIdToTrace))
                            {
                                result.Add(new ConduitTraceResult()
                                {
                                    SpanSegmentId = spanSegmentIdToTrace,
                                    SpanSegmentIds = traceInfo.UtilityNetworkTraceBySpanSegmentId[spanSegmentIdToTrace].SpanSegmentIds,
                                    OriginalTrace = traceInfo.RouteNetworkTraces.First()
                                });

                                foreach (var spanSegmentId in traceInfo.UtilityNetworkTraceBySpanSegmentId[spanSegmentIdToTrace].SpanSegmentIds)
                                    alreadyUsedSpanSegmentIds.Add(spanSegmentId);
                            }
                        }
                    }
                    // pre fixed multi conduit
                    else if (conduit.SpanStructures.Length > conduitRel.InnerConduitNumber)
                    {
                        var spanSegmentIdToTrace = conduit.SpanStructures[conduitRel.InnerConduitNumber].SpanSegments[0].Id;

                        var traceBuilder = new RouteNetworkTraceResultBuilder(_queryDispatcher, _utilityNetwork);

                        var traceInfo = traceBuilder.GetTraceInfo(new List<SpanEquipment> { conduit }, spanSegmentIdToTrace);

                        if (traceInfo != null || traceInfo.RouteNetworkTraces.Count == 1)
                        {
                            if (!alreadyUsedSpanSegmentIds.Contains(spanSegmentIdToTrace))
                            {
                                result.Add(new ConduitTraceResult()
                                {
                                    SpanSegmentId = spanSegmentIdToTrace,
                                    SpanSegmentIds = traceInfo.UtilityNetworkTraceBySpanSegmentId[spanSegmentIdToTrace].SpanSegmentIds,
                                    OriginalTrace = traceInfo.RouteNetworkTraces.First()
                                });

                                foreach (var spanSegmentId in traceInfo.UtilityNetworkTraceBySpanSegmentId[spanSegmentIdToTrace].SpanSegmentIds)
                                    alreadyUsedSpanSegmentIds.Add(spanSegmentId);
                            }
                        }
                    }
                    // Tomrør
                    else if (!spec.IsFixed && conduit.SpanStructures.Length == 1)
                    {
                        var spanSegmentIdToTrace = conduit.SpanStructures[0].SpanSegments[0].Id;

                        var traceBuilder = new RouteNetworkTraceResultBuilder(_queryDispatcher, _utilityNetwork);

                        var traceInfo = traceBuilder.GetTraceInfo(new List<SpanEquipment> { conduit }, spanSegmentIdToTrace);

                        if (traceInfo != null || traceInfo.RouteNetworkTraces.Count == 1)
                        {
                            if (!alreadyUsedSpanSegmentIds.Contains(spanSegmentIdToTrace))
                            {
                                result.Add(new ConduitTraceResult()
                                {
                                    SpanSegmentId = spanSegmentIdToTrace,
                                    SpanSegmentIds = traceInfo.UtilityNetworkTraceBySpanSegmentId[spanSegmentIdToTrace].SpanSegmentIds,
                                    OriginalTrace = traceInfo.RouteNetworkTraces.First()
                                });

                                foreach (var spanSegmentId in traceInfo.UtilityNetworkTraceBySpanSegmentId[spanSegmentIdToTrace].SpanSegmentIds)
                                    alreadyUsedSpanSegmentIds.Add(spanSegmentId);
                            }
                        }
                    }


                }
            }

            return result;
        }

        public void PrepareTables(IDbConnection conn)
        {
            ExecuteStmt(conn, "DROP TABLE IF EXISTS " + _directInRouteTableName + ";");

            ExecuteStmt(conn, "CREATE TABLE " + _directInRouteTableName + @" (
                            route_segment_id          varchar(50),
                            external_id               varchar(255));");
        }

        private void ExecuteStmt(IDbConnection conn, string sql)
        {
            Log.Debug(sql);
            var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 60 * 10; // 10 minutter
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private class SpanEquipmentForConversion
        {
            public Guid Id { get; set; }
            public string ExternalId { get; set; }
            public string ExternalSpec { get; set; }

            public List<Guid> SegmentIds = new List<Guid>();

            public List<CableConduitRel> ConduitRels = new List<CableConduitRel>();
            public Guid SpecificatonId { get; set;}

            public Guid? AccessAddressId;
            public Guid? UnitAddressId;
            public string? AddressRemark;

            public bool MissingSegments = false;


            public SpanEquipmentForConversion(Guid id, string externalId, string externalSpec, string segmentIds, string parentConduits)
            {
                Id = id;
                ExternalId = externalId;
                ExternalSpec = externalSpec;

                //  segment id's
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

                // conduit rels
                var parentConduitsSplit = parentConduits.Split(',');

                foreach (var parentConduit in parentConduitsSplit)
                {
                    var conduitRelSplit = parentConduit.Split('#');

                    ConduitRels.Add(
                        new CableConduitRel()
                        {
                            SpanEquipmentId = Guid.Parse(conduitRelSplit[0]),
                            InnerConduitNumber = Int32.Parse(conduitRelSplit[1]),
                        }
                    );
                }


                SpecificatonId = MapToSpanEquipmentSpecification(externalSpec);
            }

            private Guid MapToSpanEquipmentSpecification(string externalSpec)
            {
                if (externalSpec.Contains("24sm tkf"))
                    return TestSpecifications.FiberCable_24Fiber;
                else if (externalSpec.StartsWith("12sm"))
                    return TestSpecifications.FiberCable_12Fiber;
                else if (externalSpec.StartsWith("4smdraka"))
                    return TestSpecifications.FiberCable_4Fiber;
                else if (externalSpec.StartsWith("2smdraka"))
                    return TestSpecifications.FiberCable_2Fiber;
                else if (externalSpec.StartsWith("72smdraka"))
                    return TestSpecifications.FiberCable_72Fiber;


                throw new ApplicationException($"Cannot find spec name from external id: {externalSpec}");
            }

            public override string ToString()
            {
                string result = ExternalSpec + ": ";

                return result;
            }
         
        }

        private class ConduitTraceResult
        {
            public Guid SpanSegmentId { get; set; }

            public Guid FromNodeId { get; set; }

            public Guid ToNodeId { get; set; }

            public UtilityGraphService.API.Model.UtilityNetwork.Tracing.RouteNetworkTrace OriginalTrace { get; set; }
            public Guid[] SpanSegmentIds { get; internal set; }
        }
    }

    public class CableConduitRel
    {
        public Guid SpanEquipmentId { get; set; }
        public int InnerConduitNumber { get; set; }
    }

}
