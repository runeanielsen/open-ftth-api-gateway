using OpenFTTH.Core;
using System;

namespace OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork
{
    /// <summary>
    /// Immutable structure holding a terminal structure and its terminals. 
    /// Please keep this structure as light as possible, as millions of these guys will be exchanged and cached in memory.
    /// </summary>
    public record TerminalStructure : IIdentifiedObject
    {
        public Guid Id { get; }
        public Guid SpecificationId { get; }
        public UInt16 Position { get; }
        public Terminal[] Terminals { get; }
        public string? Description => null;
        public bool Deleted { get; init; }
        public InterfaceInfo? interfaceInfo { get; init; }

        private string? _name;

        public string? Name { 
            get {
                if (_name != null)
                    return Name;
                else
                    return Position.ToString();
            }
            set
            {
                _name = value;
            }
        }

        public TerminalStructure(Guid id, string name, Guid specificationId, ushort position, Terminal[] terminals)
        {
            Id = id;
            Name = name;
            SpecificationId = specificationId;
            Position = position;
            Terminals = terminals;
        }
    }
}
