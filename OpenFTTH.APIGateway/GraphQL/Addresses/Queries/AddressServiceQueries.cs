﻿using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Addresses.Types;
using OpenFTTH.APIGateway.Util;
using OpenFTTH.Address.API.Model;
using OpenFTTH.Address.API.Queries;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Addresses.Queries
{
    public class AddressServiceQueries : ObjectGraphType
    {
        private readonly ILogger<AddressServiceQueries> _logger;

        public AddressServiceQueries(ILogger<AddressServiceQueries> logger, IQueryDispatcher queryDispatcher, UTM32WGS84Converter coordinateConverter)
        {
            _logger = logger;

            Description = "GraphQL API for querying address information";

            FieldAsync<ListGraphType<NearestAddressSearchHitType>>(
                "nearestAccessAddresses",
                arguments:
                new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "routeNodeId" },
                    new QueryArgument<IdGraphType> { Name = "spanEquipmentOrSegmentId" },
                    new QueryArgument<FloatGraphType> { Name = "x" },
                    new QueryArgument<FloatGraphType> { Name = "y" },
                    new QueryArgument<IntGraphType> { Name = "srid" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "maxHits" }
                ),
                resolve: async (context) =>
                {
                    double x = context.GetArgument<double>("x");
                    double y = context.GetArgument<double>("y");
                    int srid = context.GetArgument<int>("srid");
                    int maxHits = context.GetArgument<int>("maxHits");
                    Guid routeNodeId = context.GetArgument<Guid>("routeNodeId");
                    Guid spanEquipmentOrSegmentId = context.GetArgument<Guid>("spanEquipmentOrSegmentId");

                    if (routeNodeId != Guid.Empty)
                    {
                        var nodeCoord = await GetNodeCoordinates(routeNodeId, queryDispatcher).ConfigureAwait(false);

                        if (nodeCoord == (0, 0))
                        {
                            context.Errors.Add(new ExecutionError($"Could not find a route node with id: {routeNodeId}"));
                            return null;
                        }

                        var getAddressInfoQuery = new GetAddressInfo(nodeCoord.Item1, nodeCoord.Item2, 25832, maxHits);

                        var result = await queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQuery);

                        if (result.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(result.Errors.First().Message));
                            return null;
                        }

                        return MapToGraphQLAddressHits(result.Value, coordinateConverter);
                    }
                    else if (spanEquipmentOrSegmentId != Guid.Empty)
                    {
                        var segmentEnd = await GetSpanSegmentEndCoordinate(spanEquipmentOrSegmentId, queryDispatcher).ConfigureAwait(false);

                        // Find address near the from span equipment end
                        var getAddressInfoQuery = new GetAddressInfo(segmentEnd.Item1, segmentEnd.Item2, 25832, maxHits);

                        var getAddressInfoQueryResult = await queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQuery).ConfigureAwait(false);
                        if (getAddressInfoQueryResult.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(getAddressInfoQueryResult.Errors.First().Message));
                            return null;
                        }

                        return MapToGraphQLAddressHits(getAddressInfoQueryResult.Value, coordinateConverter);
                    }
                    else
                    {
                        var getAddressInfoQuery = new GetAddressInfo(x, y, srid, maxHits);

                        var result = await queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQuery).ConfigureAwait(false);

                        if (result.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(result.Errors.First().Message));
                            return null;
                        }

                        return MapToGraphQLAddressHits(result.Value, coordinateConverter);
                    }
                }
           );
        }

        private async Task<(double, double)> GetNodeCoordinates(Guid nodeId, IQueryDispatcher queryDispatcher)
        {
            var routeNodeQueryResult = await queryDispatcher.HandleAsync<GetRouteNetworkDetails, Result<GetRouteNetworkDetailsResult>>(
                  new GetRouteNetworkDetails(new OpenFTTH.RouteNetwork.API.Model.RouteNetworkElementIdList() { nodeId })
                  {
                      RouteNetworkElementFilter = new RouteNetworkElementFilterOptions() { IncludeCoordinates = true }
                  }
            ).ConfigureAwait(false);

            if (routeNodeQueryResult.IsSuccess)
            {
                var etrsCoord = ConvertPointGeojsonToCoordArray(routeNodeQueryResult.Value.RouteNetworkElements.First().Coordinates);
                return (etrsCoord[0], etrsCoord[1]);
            }
            else
                return (0, 0);
        }

        private async Task<(double, double)> GetSpanSegmentEndCoordinate(Guid spanSegmentId, IQueryDispatcher queryDispatcher)
        {
            // Query span equipment
            var equipmentQueryResult = await queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                new GetEquipmentDetails(new EquipmentIdList() { spanSegmentId })
                {
                    EquipmentDetailsFilter = new EquipmentDetailsFilterOptions { IncludeRouteNetworkTrace = true }
                }
            ).ConfigureAwait(false);

            if (equipmentQueryResult.IsFailed)
            {
                throw new ApplicationException($"Error querying span equipment by id: {spanSegmentId} " + equipmentQueryResult.Errors.First().Message);
            }

            if (equipmentQueryResult.Value.SpanEquipment == null || equipmentQueryResult.Value.SpanEquipment.Count == 0)
            {
                throw new ApplicationException($"Cannot find any span equipment containing a span segment with id: {spanSegmentId}");
            }

            Guid spanEquipmentInterestId = equipmentQueryResult.Value.SpanEquipment.First().WalkOfInterestId;

            // Query route network
            var routeNetworkQueryResult = await queryDispatcher.HandleAsync<GetRouteNetworkDetails, FluentResults.Result<GetRouteNetworkDetailsResult>>(
                      new GetRouteNetworkDetails(new InterestIdList() { spanEquipmentInterestId })
                      {
                          RouteNetworkElementFilter = new RouteNetworkElementFilterOptions()
                          {
                              IncludeCoordinates = true,
                          },
                          RelatedInterestFilter = RelatedInterestFilterOptions.None
                      }
            ).ConfigureAwait(false);

            if (routeNetworkQueryResult.IsFailed)
            {
                throw new ApplicationException($"Got error querying interest information for span equipment: {spanSegmentId} ERROR: " + routeNetworkQueryResult.Errors.First().Message);
            }

            if (routeNetworkQueryResult.Value.Interests == null || !routeNetworkQueryResult.Value.Interests.ContainsKey(spanEquipmentInterestId))
                throw new ApplicationException($"Got no info querying interest information for span equipment: {spanSegmentId}");

            var routeNetworkElementIds = routeNetworkQueryResult.Value.Interests[spanEquipmentInterestId].RouteNetworkElementRefs;

            var lastNodeInSpanSegment = routeNetworkElementIds.Last();

            var coord = await GetNodeCoordinates(lastNodeInSpanSegment, queryDispatcher).ConfigureAwait(false);

            _logger.LogInformation($"Address search info: Get coordinate of span segment: {spanSegmentId} in span equipment: {equipmentQueryResult.Value.SpanEquipment.First().Name} {equipmentQueryResult.Value.SpanEquipment.First().Id} Route node id: {lastNodeInSpanSegment} successfully returned: x={coord.Item1} y={coord.Item2}");

            return coord;
        }

        private double[] ConvertPointGeojsonToCoordArray(string geojson)
        {
            var geojsonSplit = geojson.Replace("[", "").Replace("]", "").Split(',');
            if (geojsonSplit.Length != 2)
                throw new ApplicationException($"Expected point geojson, but got: '{geojson}'");

            var result = new double[2];
            result[0] = Double.Parse(geojsonSplit[0], CultureInfo.InvariantCulture);
            result[1] = Double.Parse(geojsonSplit[1], CultureInfo.InvariantCulture);

            return result;
        }

        private List<NearestAddressSearchHit> MapToGraphQLAddressHits(GetAddressInfoResult addressQueryResult, UTM32WGS84Converter coordinateConverter)
        {
            List<NearestAddressSearchHit> result = new();

            foreach (var addressHit in addressQueryResult.AddressHits)
            {
                result.Add(new NearestAddressSearchHit()
                {
                    AccessAddress = MapAccessAddress(addressHit.Key, addressQueryResult, coordinateConverter),
                    Distance = addressHit.Distance.Value
                });
            }

            return result;
        }

        public static AccessAddressData MapAccessAddress(Guid accessAddressId, GetAddressInfoResult addressQueryResult, UTM32WGS84Converter coordinateConverter)
        {
            var searchAccessAddress = addressQueryResult.AccessAddresses[accessAddressId];

            var wgs84Coord = coordinateConverter.ConvertFromUTM32NToWGS84(searchAccessAddress.AddressPoint.X, searchAccessAddress.AddressPoint.Y);

            List<UnitAddress> unitAddresses = new();

            foreach (var unitAddress in addressQueryResult.UnitAddresses)
            {
                if (unitAddress.AccessAddressId == searchAccessAddress.Id)
                    unitAddresses.Add(unitAddress);
            }

            var mappedAccessAddress = new AccessAddressData()
            {
                Id = searchAccessAddress.Id,
                ExternalId = searchAccessAddress.ExternalId,
                HouseNumber = searchAccessAddress.HouseNumber,
                PostDistrictCode = searchAccessAddress.PostDistrictCode,
                PostDistrict = searchAccessAddress.PostDistrict,
                RoadCode = searchAccessAddress.RoadCode,
                RoadName = searchAccessAddress.RoadName,
                TownName = searchAccessAddress.TownName,
                MunicipalCode = searchAccessAddress.MunicipalCode,
                Xetrs = searchAccessAddress.AddressPoint.X,
                Yetrs = searchAccessAddress.AddressPoint.Y,
                Xwgs = wgs84Coord[0],
                Ywgs = wgs84Coord[1],
                UnitAddresses = unitAddresses
            };

            return mappedAccessAddress;
        }
    }
}
