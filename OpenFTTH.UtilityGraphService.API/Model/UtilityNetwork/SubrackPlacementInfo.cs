using System;

namespace OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork
{
    public record SubrackPlacementInfo
    {
        public Guid RackId { get; set; }
        public int StartUnitPosition { get; set; }
        public SubrackPlacmentMethod PlacementMethod { get; set; }

        public SubrackPlacementInfo(Guid rackId, int startUnitPosition, SubrackPlacmentMethod placementMethod)
        {
            RackId = rackId;
            StartUnitPosition = startUnitPosition;
            PlacementMethod = placementMethod;
        }
    }
}
