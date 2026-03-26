using System;
using System.Collections.Generic;

namespace OpenFTTH.UtilityGraphService.Business.Trace
{
    public record SegmentWalk
    {
        public Guid SpanEquipmentOrSegmentId { get; }

        public List<SegmentWalkHop> Hops = new();

        public string[]? Tags;

        public SegmentWalk(Guid spanEquipmentOrSegmentId, string[]? tags)
        {
            SpanEquipmentOrSegmentId = spanEquipmentOrSegmentId;
            Tags = tags;
        }
    }
}
