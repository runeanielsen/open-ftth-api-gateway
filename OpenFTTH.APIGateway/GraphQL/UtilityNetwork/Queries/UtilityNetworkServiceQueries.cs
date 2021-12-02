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

            Field<ListGraphType<TerminalEquipmentSpecificationType>>(
                name: "terminalEquipmentSpecifications",
                description: "Retrieve all terminal equipment specifications.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetTerminalEquipmentSpecifications,
                        Result<LookupCollection<TerminalEquipmentSpecification>>>(new GetTerminalEquipmentSpecifications()).Result;

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

            Field<ListGraphType<RackSpecificationType>>(
               name: "rackSpecifications",
               description: "Retrieve all rack specifications.",
               resolve: context =>
               {
                   var queryResult = queryDispatcher.HandleAsync<GetRackSpecifications, Result<LookupCollection<RackSpecification>>>(new GetRackSpecifications()).Result;

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



            Field<NodeContainerType>(
              name: "nodeContainer",
              description: "Query information related to a specific node container",
              arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerId" }
              ),
              resolve: context =>
              {
                  var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");

                   // Get equipment information
                   var equipmentQueryResult = queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                      new GetEquipmentDetails(new InterestIdList() { nodeContainerId })
                  ).Result;

                  if (equipmentQueryResult.IsFailed)
                  {
                      foreach (var error in equipmentQueryResult.Errors)
                          context.Errors.Add(new ExecutionError(error.Message));

                      return null;
                  }

                  if (equipmentQueryResult.Value.NodeContainers == null || equipmentQueryResult.Value.NodeContainers.Count == 0)
                  {
                      context.Errors.Add(new ExecutionError($"Cannot find any node container with id: {nodeContainerId}"));

                      return null;
                  }


                  var nodeContainer = equipmentQueryResult.Value.NodeContainers.First();

                  return nodeContainer;
              }
          );


          Field<RackType>(
            name: "rack",
            description: "Query information related to a specific rack residing within a node container",
            arguments: new QueryArguments(
              new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "nodeContainerId" },
              new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "rackId" }
            ),
            resolve: context =>
            {
                var nodeContainerId = context.GetArgument<Guid>("nodeContainerId");
                var rackId = context.GetArgument<Guid>("rackId");

                // Get equipment information
                var equipmentQueryResult = queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                     new GetEquipmentDetails(new InterestIdList() { nodeContainerId })
                 ).Result;

                if (equipmentQueryResult.IsFailed)
                {
                    foreach (var error in equipmentQueryResult.Errors)
                        context.Errors.Add(new ExecutionError(error.Message));

                    return null;
                }

                if (equipmentQueryResult.Value.NodeContainers == null || equipmentQueryResult.Value.NodeContainers.Count == 0)
                {
                    context.Errors.Add(new ExecutionError($"Cannot find any node container with id: {nodeContainerId}"));
                    return null;
                }

                var nodeContainer = equipmentQueryResult.Value.NodeContainers.First();


                if (nodeContainer.Racks == null || !nodeContainer.Racks.Any(r => r.Id == rackId))
                {
                    context.Errors.Add(new ExecutionError($"Cannot find any rack with id: {rackId} within node container with id: {nodeContainerId}"));
                    return null;
                }

                return nodeContainer.Racks.First(r => r.Id == rackId);
            }
        );
        
        }
    }
}
