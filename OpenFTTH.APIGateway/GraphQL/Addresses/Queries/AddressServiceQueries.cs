using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.Address.API.Model;
using OpenFTTH.Address.API.Queries;
using OpenFTTH.APIGateway.GraphQL.Addresses.Types;
using OpenFTTH.APIGateway.Util;
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

        public AddressServiceQueries(ILogger<AddressServiceQueries> logger, IQueryDispatcher queryDispatcher)
        {
            _logger = logger;

            Description = "GraphQL API for querying address information";

            Field<ListGraphType<NearestAddressSearchHitType>>(
                "nearestAccessAddresses",
                arguments:
                new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "routeNodeId" },
                    new QueryArgument<IdGraphType> { Name = "spanSegmentId" },
                    new QueryArgument<FloatGraphType> { Name = "x" },
                    new QueryArgument<FloatGraphType> { Name = "y" },
                    new QueryArgument<IntGraphType> { Name = "srid" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "maxHits" }
                ),
                resolve: context =>
                {
                    double x = context.GetArgument<double>("x");
                    double y = context.GetArgument<double>("y");
                    int srid = context.GetArgument<int>("srid");
                    int maxHits = context.GetArgument<int>("maxHits");
                    Guid routeNodeId = context.GetArgument<Guid>("routeNodeId");
                    Guid spanSegmentId = context.GetArgument<Guid>("spanSegmentId");

                    if (routeNodeId != Guid.Empty)
                    {
                        var nodeCoord = GetNodeCoordinates(routeNodeId, queryDispatcher);

                        if (nodeCoord == (0, 0))
                        {
                            context.Errors.Add(new ExecutionError($"Could not find a route node with id: {routeNodeId}"));
                            return null;
                        }

                        var getAddressInfoQuery = new GetAddressInfo(nodeCoord.Item1, nodeCoord.Item2, 25832, maxHits);

                        var result = queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQuery).Result;

                        if (result.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(result.Errors.First().Message));
                            return null;
                        }

                        return MapToGraphQLAddressHits(result.Value);
                    }
                    else if (spanSegmentId != Guid.Empty)
                    {
                        var segmentEnds = GetSpanSegmentEndCoordinates(spanSegmentId, queryDispatcher);


                        // Find address near the from span equipment end
                        var getAddressInfoQueryFromEnd = new GetAddressInfo(segmentEnds[0].Item1, segmentEnds[0].Item2, 25832, maxHits / 2);

                        var fromEndResult = queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQueryFromEnd).Result;

                        if (fromEndResult.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(fromEndResult.Errors.First().Message));
                            return null;
                        }

                        // Find address near the to span equipment end
                        var getAddressInfoQueryToEnd = new GetAddressInfo(segmentEnds[1].Item1, segmentEnds[1].Item2, 25832, maxHits / 2);

                        var toEndResult = queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQueryToEnd).Result;

                        if (toEndResult.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(fromEndResult.Errors.First().Message));
                            return null;
                        }

                        var hits = MapToGraphQLAddressHits(fromEndResult.Value);
                        hits.AddRange(MapToGraphQLAddressHits(toEndResult.Value));

                        return hits;
                    }
                    else
                    {
                        var getAddressInfoQuery = new GetAddressInfo(x, y, srid, maxHits);

                        var result = queryDispatcher.HandleAsync<GetAddressInfo, Result<GetAddressInfoResult>>(getAddressInfoQuery).Result;

                        if (result.IsFailed)
                        {
                            context.Errors.Add(new ExecutionError(result.Errors.First().Message));
                            return null;
                        }

                        return MapToGraphQLAddressHits(result.Value);
                    }


                }
           );
        }

        private (double, double) GetNodeCoordinates(Guid nodeId, IQueryDispatcher queryDispatcher)
        {

            var routeNodeQueryResult = queryDispatcher.HandleAsync<GetRouteNetworkDetails, Result<GetRouteNetworkDetailsResult>>(
                  new GetRouteNetworkDetails(new OpenFTTH.RouteNetwork.API.Model.RouteNetworkElementIdList() { nodeId })
                  {
                      RouteNetworkElementFilter = new RouteNetworkElementFilterOptions() { IncludeCoordinates = true }
                  }
              ).Result;

            if (routeNodeQueryResult.IsSuccess)
            {
                var etrsCoord = ConvertPointGeojsonToCoordArray(routeNodeQueryResult.Value.RouteNetworkElements.First().Coordinates);

                return (etrsCoord[0], etrsCoord[1]);
            }
            else
                return (0, 0);
        }

        private List<(double, double)> GetSpanSegmentEndCoordinates(Guid spanSegmentId, IQueryDispatcher queryDispatcher)
        {
            // Query span equipment
            var equipmentQueryResult = queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                new GetEquipmentDetails(new EquipmentIdList() { spanSegmentId })
                {
                    EquipmentDetailsFilter = new EquipmentDetailsFilterOptions { IncludeRouteNetworkTrace = true }
                }
            ).Result;

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
            var routeNetworkQueryResult = queryDispatcher.HandleAsync<GetRouteNetworkDetails, FluentResults.Result<GetRouteNetworkDetailsResult>>(
                      new GetRouteNetworkDetails(new InterestIdList() { spanEquipmentInterestId })
                      {
                          RouteNetworkElementFilter = new RouteNetworkElementFilterOptions()
                          { 
                              IncludeCoordinates = true,
                          },
                          RelatedInterestFilter = RelatedInterestFilterOptions.None
                      }
                  ).Result;

            if (routeNetworkQueryResult.IsFailed)
            {
                throw new ApplicationException($"Got error querying interest information for span equipment: {spanSegmentId} ERROR: " + routeNetworkQueryResult.Errors.First().Message);
            }

            if (routeNetworkQueryResult.Value.Interests == null || !routeNetworkQueryResult.Value.Interests.ContainsKey(spanEquipmentInterestId))
                throw new ApplicationException($"Got no info querying interest information for span equipment: {spanSegmentId}");

            var routeNetworkElementIds = routeNetworkQueryResult.Value.Interests[spanEquipmentInterestId].RouteNetworkElementRefs;

            var result = new List<(double, double)>();

            result.Add(GetNodeCoordinates(routeNetworkElementIds.First(), queryDispatcher));
            result.Add(GetNodeCoordinates(routeNetworkElementIds.Last(), queryDispatcher));

            return result;
        }

        private double[] ConvertPointGeojsonToCoordArray(string geojson)
        {
            List<double> result = new();

            var geojsonSplit = geojson.Replace("[", "").Replace("]", "").Split(',');

            foreach (var coord in geojsonSplit)
            {
                result.Add(Double.Parse(coord, CultureInfo.InvariantCulture));
            }

            if (result.Count != 2)
                throw new ApplicationException($"Expected point geojson, but got: '{geojson}'");

            return result.ToArray();
        }



        private List<NearestAddressSearchHit> MapToGraphQLAddressHits(GetAddressInfoResult addressQueryResult)
        {
            List<NearestAddressSearchHit> result = new();

            foreach (var addressHit in addressQueryResult.AddressHits)
            {
                result.Add(new NearestAddressSearchHit()
                {
                    AccessAddress = MapAccessAddress(addressHit.Key, addressQueryResult),
                    Distance = addressHit.Distance.Value
                });
            }

            return result;
        }

        public static AccessAddressData MapAccessAddress(Guid accessAddressId, GetAddressInfoResult addressQueryResult)
        {
            var searchAccessAddress = addressQueryResult.AccessAddresses[accessAddressId];

            var wgs84Coord = UTM32WGS84Converter.ConvertFromUTM32NToWGS84(searchAccessAddress.AddressPoint.X, searchAccessAddress.AddressPoint.Y);

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
