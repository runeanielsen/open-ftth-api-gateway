using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types;
using OpenFTTH.APIGateway.Util;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Tracing;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Queries
{
    public class UtilityNetworkServiceQueries : ObjectGraphType
    {
        public UtilityNetworkServiceQueries(ILogger<UtilityNetworkServiceQueries> logger, IQueryDispatcher queryDispatcher)
        {
            Description = "GraphQL API for querying data owned by utility network service";

            Field<ListGraphType<ManufacturerType>>(
                name: "manufacturers",
                description: "Retrieve all manufacturers.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetManufacturer, Result<LookupCollection<Manufacturer>>>(new GetManufacturer()).Result;

                    return queryResult.Value;
                }
            );

            Field<ListGraphType<SpanEquipmentSpecificationType>>(
                name: "spanEquipmentSpecifications",
                description: "Retrieve all span equipment specifications.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetSpanEquipmentSpecifications,
                        Result<LookupCollection<SpanEquipmentSpecification>>>(new GetSpanEquipmentSpecifications()).Result;

                    return queryResult.Value.OrderBy(s => s.Description);
                }
            );

            Field<ListGraphType<NodeContainerSpecificationType>>(
                name: "nodeContainerSpecifications",
                description: "Retrieve all node container specifications.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetNodeContainerSpecifications, Result<LookupCollection<NodeContainerSpecification>>>(new GetNodeContainerSpecifications()).Result;

                    return queryResult.Value.OrderBy(s => s.Description);
                }
            );

            Field<SpanSegmentTraceType>(
                name: "spanSegmentTrace",
                description: "Trace a specific span segment",
                arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanSegmentId" }
                ),
                resolve: context =>
                {
                    var spanSegmentId = context.GetArgument<Guid>("spanSegmentId");

                    // Get equipment information
                    var equipmentQueryResult = queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                        new GetEquipmentDetails(new EquipmentIdList() { spanSegmentId })
                        {
                            EquipmentDetailsFilter = new EquipmentDetailsFilterOptions { IncludeRouteNetworkTrace = true }
                        }
                    ).Result;

                    if (equipmentQueryResult.IsFailed)
                    {
                        foreach (var error in equipmentQueryResult.Errors)
                            context.Errors.Add(new ExecutionError(error.Message));

                        return null;
                    }

                    if (equipmentQueryResult.Value.SpanEquipment == null || equipmentQueryResult.Value.SpanEquipment.Count == 0)
                    {
                        context.Errors.Add(new ExecutionError($"Cannot find any span equipment containing a span segment with id: {spanSegmentId}"));
                        return null;
                    }

                    if (equipmentQueryResult.Value.RouteNetworkTraces == null)
                    {
                        context.Errors.Add(new ExecutionError($"No trace information returned for span segment with id: {spanSegmentId}"));
                        return null;
                    }

                    if (equipmentQueryResult.Value.RouteNetworkTraces.Count != 1)
                    {
                        context.Errors.Add(new ExecutionError($"Expected only one trace returned for span segment with id: {spanSegmentId}. Are you sure you did a query on a span segment id and not a span equipment id?"));
                        return null;
                    }

                    var theTrace = equipmentQueryResult.Value.RouteNetworkTraces.First();

                    return new SpanSegmentTrace()
                    {
                        RouteNetworkSegmentIds = theTrace.RouteSegmentIds,
                        RouteNetworkSegmentGeometries = UTM32WGS84Converter.ConvertGeoJsonLineStringsToWgs84(theTrace.RouteSegmentGeometries)
                    };
                }
            );


            Field<SpanEquipmentType>(
               name: "spanEquipment",
               description: "Query information related to a specific span equipment",
               arguments: new QueryArguments(
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "spanEquipmentOrSegmentId" }
               ),
               resolve: context =>
               {
                   var spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");

                   // Get equipment information
                   var equipmentQueryResult = queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                       new GetEquipmentDetails(new EquipmentIdList() { spanEquipmentOrSegmentId })
                   ).Result;

                   if (equipmentQueryResult.IsFailed)
                   {
                       foreach (var error in equipmentQueryResult.Errors)
                           context.Errors.Add(new ExecutionError(error.Message));

                       return null;
                   }

                   if (equipmentQueryResult.Value.SpanEquipment == null || equipmentQueryResult.Value.SpanEquipment.Count == 0)
                   {
                       context.Errors.Add(new ExecutionError($"Cannot find any span equipment containing a span segment with id: {spanEquipmentOrSegmentId}"));

                       return null;
                   }


                   var spanEquipment = equipmentQueryResult.Value.SpanEquipment.First();

                   return spanEquipment;
               }
           );
        }

        
    }
}
