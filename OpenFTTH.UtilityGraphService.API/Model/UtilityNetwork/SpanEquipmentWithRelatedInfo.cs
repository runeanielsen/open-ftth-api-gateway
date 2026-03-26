using System;
using System.Collections.Generic;

namespace OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork
{
    public record SpanEquipmentWithRelatedInfo : SpanEquipment
    {
        public SpanSegmentRouteNetworkTraceRef[]? RouteNetworkTraceRefs { get; set; }

        public Dictionary<Guid, string[]> TagsByBySpanSegmentId = new();

        public string? UpstreamLabel { get; set; }

        public string? DownstreamLabel { get; set; }

        public SpanEquipmentWithRelatedInfo(SpanEquipment original) : base(original) { }
    }
}
