using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using System;
using System.Collections.Generic;

namespace OpenFTTH.APIGateway.Specifications
{
    public class Specifications
    {
        public List<ManufacturerSpec> Manufacturers { get; set; }
        public List<TerminalStructureSpec> TerminalStructures { get; set; }
        public List<TerminalEquipmentSpec> TerminalEquipments { get; set; }
        public List<SpanEquipmentSpec> SpanEquipments { get; set; }
        public List<SpanStructureSpec> SpanStructure { get; set; }
        public List<NodeContainerSpec> NodeContainers { get; set; }
    }

    public record ManufacturerSpec
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Deprecated { get; set; }
    }

    public record TerminalEquipmentSpec
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortName { get; set; }
        public string? Description { get; set; }
        public List<string>? Manufacturers { get; init; }
        public bool IsRackEquipment { get; set; }
        public int HeightInRackUnits { get; set; }
        public bool IsFixed { get; set; }
        public bool IsAddressable { get; init; }
        public bool IsCustomerTermination { get; init; }
        public bool IsLineTermination { get; init; }
        public bool Deprecated { get; set; }
        public List<TerminalStructureTemplateSpec> Structures { get; set; }
    }

    public class TerminalStructureTemplateSpec
    {
        public UInt16 Position { get; set; }
        public string RefName { get; set; }
    }

    public record TerminalStructureSpec
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortName { get; set; }
        public string? Description { get; set; }
        public List<string>? Manufacturers { get; set; }
        public List<TerminalTemplateSpec> Terminals { get; set; }
        public bool Deprecated { get; init; }
    }

    public record TerminalTemplateSpec
    {
        public string Name { get; set; }
        public string Direction { get; set; }
        public bool IsPigtail { get; set; }
        public bool IsSplice { get; set; }
        public string? ConnectorType { get; set; }
        public string? InternalConnectivityNode { get; set; }
    }



    public record NodeContainerSpec
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortName { get; set; }
        public string? Description { get; set; }
        public List<string>? Manufacturers { get; set; }
        public bool Deprecated { get; init; }
    }

    public record SpanEquipmentSpec
    {
        //public Guid Id { get; }
        public string Category { get; set; }
        public string Name { get; set; }
        public bool Deprecated { get; set; }
        public bool IsFixed { get; set; }
        public bool IsMultiLevel { get; set; }
        public string? Description { get; set; }
        public List<string>? Manufacturers { get; set; }
        public bool IsCable { get; set; }

        public SpanStructureTemplateSpec RootStructure { get; set; }
    }

    public class SpanStructureTemplateSpec
    {
        public UInt16 Level { get; set; }
        public UInt16 Position { get; set; }
        public string RefName { get; set; }

        public List<SpanStructureTemplateSpec>? Children { get; set; }
    }

    public record SpanStructureSpec
    {
        //public Guid Id { get; }
        public string SpanClassType { get; }
        public string Name { get; }
        public string Color { get; }
        public int? InnerDiameter { get; init; }
        public int? OuterDiameter { get; init; }
        public bool Deprecated { get; init; }
        public string? Description { get; init; }
    }
}
