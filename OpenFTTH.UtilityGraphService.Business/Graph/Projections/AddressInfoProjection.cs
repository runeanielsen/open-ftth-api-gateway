using OpenFTTH.Core.Address;
using OpenFTTH.Core.Address.Events;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.EventSourcing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenFTTH.UtilityGraphService.Business.Graph.Projections
{
    public class AddressInfoProjection : ProjectionBase
    {
        private readonly ConcurrentDictionary<Guid, AccessAddressRecord> _accessAddressById = new();

        private readonly ConcurrentDictionary<Guid, UnitAddressRecord> _unitAddressById = new();

        private readonly ConcurrentDictionary<Guid, RoadRecord> _roadById = new();

        public AddressInfoProjection()
        {
            ProjectEventAsync<AccessAddressCreated>(ProjectAsync);
            ProjectEventAsync<AccessAddressPendingOfficialChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressStatusChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressExternalIdChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressCoordinateChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressHouseNumberChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressRoadIdChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressRoadCodeChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressPostCodeIdChanged>(ProjectAsync);
            ProjectEventAsync<AccessAddressMunicipalCodeChanged>(ProjectAsync);

            ProjectEventAsync<UnitAddressCreated>(ProjectAsync);
            ProjectEventAsync<UnitAddressDeleted>(ProjectAsync);
            ProjectEventAsync<UnitAddressPendingOfficialChanged>(ProjectAsync);
            ProjectEventAsync<UnitAddressStatusChanged>(ProjectAsync);
            ProjectEventAsync<UnitAddressExternalIdChanged>(ProjectAsync);
            ProjectEventAsync<UnitAddressSuiteNameChanged>(ProjectAsync);
            ProjectEventAsync<UnitAddressFloorNameChanged>(ProjectAsync);
            ProjectEventAsync<UnitAddressAccessAddressIdChanged>(ProjectAsync);

            ProjectEventAsync<RoadCreated>(ProjectAsync);
            ProjectEventAsync<RoadDeleted>(ProjectAsync);
            ProjectEventAsync<RoadStatusChanged>(ProjectAsync);
            ProjectEventAsync<RoadExternalIdChanged>(ProjectAsync);
            ProjectEventAsync<RoadNameChanged>(ProjectAsync);

        }

        public IReadOnlyDictionary<Guid, AccessAddressRecord> AccessAddressesById
        {
            get { return _accessAddressById; }
        }

        public IReadOnlyDictionary<Guid, UnitAddressRecord> UnitAddressesById
        {
            get { return _unitAddressById; }
        }

        public IReadOnlyDictionary<Guid, RoadRecord> RoadsById
        {
            get { return _roadById; }
        }

        public AddressInfo? GetAddressInfo(Guid unitAddressId)
        {
            if (_unitAddressById.TryGetValue(unitAddressId, out UnitAddressRecord unitAddressRecord))
            {
                return new AddressInfo()
                {
                    AccessAddressId = unitAddressRecord.AccessAddressId,
                    UnitAddressId = unitAddressRecord.Id
                };

            }

            return null;
        }

        private Task ProjectAsync(IEventEnvelope eventEnvelope)
        {
            switch (eventEnvelope.Data)
            {
                /////////////////////////////////////////////
                // Access address events

                case (AccessAddressCreated @event):
                    _accessAddressById.TryAdd(@event.Id,
                        new AccessAddressRecord(@event.Id)
                        {
                            ExternalId = @event.ExternalId,
                            PendingOfficial = @event.PendingOfficial,
                            Status = @event.Status,
                            EastCoordinate = @event.EastCoordinate,
                            NorthCoordinate = @event.NorthCoordinate,
                            HouseNumber = @event.HouseNumber,
                            TownName = @event.TownName,
                            PostDistrictCode = @event.RoadCode,
                            MunicipalCode = @event.MunicipalCode,
                            RoadCode = @event.RoadCode,
                            RoadId = @event.RoadId
                        });
                    break;

                case (AccessAddressPendingOfficialChanged @event):
                    _accessAddressById[@event.Id].PendingOfficial = @event.PendingOfficial;
                    break;

                case (AccessAddressStatusChanged @event):
                    _accessAddressById[@event.Id].Status = @event.Status;
                    break;

                case (AccessAddressExternalIdChanged @event):
                    _accessAddressById[@event.Id].ExternalId = @event.ExternalId;
                    break;

                case (AccessAddressCoordinateChanged @event):
                    var accessAddressToUpdate = _accessAddressById[@event.Id];
                    accessAddressToUpdate.EastCoordinate = @event.EastCoordinate;
                    accessAddressToUpdate.NorthCoordinate = @event.NorthCoordinate;
                    break;
           
                case (AccessAddressHouseNumberChanged @event):
                    _accessAddressById[@event.Id].HouseNumber = @event.HouseNumber;
                    break;
           
                case (AccessAddressRoadIdChanged @event):
                    _accessAddressById[@event.Id].RoadId = @event.RoadId;
                    break;

                case (AccessAddressRoadCodeChanged @event):
                    _accessAddressById[@event.Id].RoadCode = @event.RoadCode;
                    break;

                case (AccessAddressPostCodeIdChanged @event):
                    _accessAddressById[@event.Id].PostCodeId = @event.PostCodeId;
                    break;

                case (AccessAddressMunicipalCodeChanged @event):
                    _accessAddressById[@event.Id].MunicipalCode = @event.MunicipalCode;
                    break;

                /////////////////////////////////////////////
                // Unit address events

                case (UnitAddressCreated @event):
                    _unitAddressById.TryAdd(@event.Id,
                        new UnitAddressRecord(@event.Id)
                        {
                            ExternalId = @event.ExternalId,
                            AccessAddressId = @event.AccessAddressId,
                            PendingOfficial = @event.PendingOfficial,
                            Status = @event.Status,
                            FloorName = @event.FloorName,
                            SuitName = @event.SuiteName
                        });

                    _accessAddressById[@event.AccessAddressId].UnitAddressIds.Add(@event.Id);
                    break;

                case (UnitAddressDeleted @event):
                    _accessAddressById[_unitAddressById[@event.Id].AccessAddressId].UnitAddressIds.Remove(@event.Id);
                    _unitAddressById.TryRemove(@event.Id, out var _);
                    break;

                case (UnitAddressPendingOfficialChanged @event):
                    _unitAddressById[@event.Id].PendingOfficial = @event.PendingOfficial;
                    break;

                case (UnitAddressStatusChanged @event):
                    _unitAddressById[@event.Id].Status = @event.Status;
                    break;

                case (UnitAddressExternalIdChanged @event):
                    _unitAddressById[@event.Id].ExternalId = @event.ExternalId;
                    break;

                case (UnitAddressSuiteNameChanged @event):
                    _unitAddressById[@event.Id].SuitName = @event.SuiteName;
                    break;

                case (UnitAddressFloorNameChanged @event):
                    _unitAddressById[@event.Id].FloorName = @event.FloorName;
                    break;

                case (UnitAddressAccessAddressIdChanged @event):
                    var oldAccessAddressId = _unitAddressById[@event.Id].AccessAddressId;
                    _accessAddressById[oldAccessAddressId].UnitAddressIds.Remove(@event.Id);

                    _unitAddressById[@event.Id].AccessAddressId = @event.AccessAddressId;
                    _accessAddressById[@event.AccessAddressId].UnitAddressIds.Add(@event.Id);
                    break;


                /////////////////////////////////////////////
                // Road events

                case (RoadCreated @event):
                    _roadById.TryAdd(@event.Id,
                        new RoadRecord(@event.Id)
                        {
                            Status = @event.Status,
                            ExternalId = @event.ExternalId,
                            Name = @event.Name
                        });
                    break;

                case (RoadDeleted @event):
                    _roadById.TryRemove(@event.Id, out var _);
                    break;

                case (RoadExternalIdChanged @event):
                    _roadById[@event.Id].ExternalId = @event.ExternalId;
                    break;

                case (RoadNameChanged @event):
                    _roadById[@event.Id].Name = @event.Name;
                    break;

            }

            return Task.CompletedTask;
        }
    }

    public record AccessAddressRecord
    {
        public AccessAddressRecord(Guid id)
        {
            Id = id;
            UnitAddressIds = [];
        }

        public Guid Id { get; }
        public string? ExternalId { get; set; }
        public bool PendingOfficial { get; set; }
        public AccessAddressStatus Status { get; internal set; }
        public double EastCoordinate { get; set; }
        public double NorthCoordinate { get; set; }
        public string? HouseNumber { get; set; }
        public Guid PostCodeId { get; set; }
        public string? PostDistrictCode { get; set; }
        public string? RoadCode { get; set; }
        public Guid? RoadId { get; set; }
        public string? TownName { get; set; }
        public string? MunicipalCode { get; set; }
        public List<Guid> UnitAddressIds { get; set; }
    }

    public record UnitAddressRecord
    {
        public UnitAddressRecord(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public string? ExternalId { get; set; }
        public bool PendingOfficial { get; set; }
        public UnitAddressStatus Status { get; internal set; }
        public Guid AccessAddressId { get; set; }
        public string? FloorName { get; set; }
        public string? SuitName { get; set; }
    }

    public record RoadRecord
    {
        public RoadRecord(Guid id)
        {
            Id = Id;
        }

        public Guid Id { get; init; }
        public string? ExternalId { get; set; }
        public string? Name { get; set; }
        public RoadStatus Status { get; set; }
    }
}




