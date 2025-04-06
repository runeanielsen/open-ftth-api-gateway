using OpenFTTH.Core;
using System;

namespace OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork
{
    /// <summary>
    /// Used as part of a span equipment specification to specify cable tubes
    /// </summary>
    public record CableTubeSpecification
    {
        public UInt16 Position { get; }
        public string Color { get; }
        public string? Description { get; init; }

        public CableTubeSpecification(UInt16 position, string color, string? description)
        {
            this.Position = position;
            this.Color = color;
            this.Description = description;
        }
    }
}
