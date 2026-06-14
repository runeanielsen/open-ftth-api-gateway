using System;

namespace OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork
{
    public record EquipmentTag
    {
        public Guid TerminalOrSpanId { get; set; }
        public string[]? Tags { get; set; }
        public string? Comment { get; set; }

        public EquipmentTag(Guid terminalOrSpanId, string[] tags, string? comment = null)
        {
            TerminalOrSpanId = terminalOrSpanId;
            Tags = tags;
            Comment = comment;
        }
    }

    public record EquipmentDisplayTag
    {
        public Guid TerminalOrSpanId { get; set; }
        public string DisplayName { get; set; }
        public string[]? Tags { get; set; }
        public string? Comment { get; set; }

        public EquipmentDisplayTag(Guid terminalOrSpanId, string displayName, string[]? tags, string? comment = null)
        {
            TerminalOrSpanId = terminalOrSpanId;
            DisplayName = displayName;
            Tags = tags;
            Comment = comment;
        }
    }
}
