using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.Business.NodeContainers.Projections;
using OpenFTTH.UtilityGraphService.Business.SpanEquipments.Projections;
using OpenFTTH.UtilityGraphService.Business.TerminalEquipments;
using OpenFTTH.UtilityGraphService.Business.TerminalEquipments.Projections;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Xml.Linq;

namespace OpenFTTH.APIGateway.Specifications
{
    public class SpecificationExporter
    {
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;
        private IEventStore _eventStore;

        private ILogger<SpecificationExporter> _logger;

        public SpecificationExporter(ILoggerFactory loggerFactory, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IEventStore eventStore)
        {
            _logger = loggerFactory.CreateLogger<SpecificationExporter>();
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _eventStore = eventStore;
        }

        public void Export(string fileName)
        {
            var manufacturerSpecs = _eventStore.Projections.Get<ManufacturerProjection>().Manufacturer;

            var terminalEquipmentSpecs = _eventStore.Projections.Get<TerminalEquipmentSpecificationsProjection>().Specifications;

            var terminalStructureSpecs = _eventStore.Projections.Get<TerminalStructureSpecificationsProjection>().Specifications;

            var nodeContainerSpecs = _eventStore.Projections.Get<NodeContainerSpecificationsProjection>().Specifications;

            var specificationData = new Specifications();

            // Manufactures
            specificationData.Manufacturers = new List<ManufacturerSpec>();

            foreach (var manufacturerSpec in manufacturerSpecs)
            {
                specificationData.Manufacturers.Add(
                    new ManufacturerSpec()
                    {
                        //Id = manufacturer.Id,
                        Name = manufacturerSpec.Name,
                        Description = manufacturerSpec.Description,
                        Deprecated = manufacturerSpec.Deprecated
                    }
                );
            }

            // Node container specs
            specificationData.NodeContainers = new List<NodeContainerSpec>();

            foreach (var nodeContainerSpec in nodeContainerSpecs)
            {
                specificationData.NodeContainers.Add(
                    new NodeContainerSpec()
                    {
                        Name = nodeContainerSpec.Description,
                        Category = nodeContainerSpec.Category,
                        ShortName = nodeContainerSpec.Name,
                        Manufacturers = GetManufacturesFromIds(manufacturerSpecs, nodeContainerSpec.ManufacturerRefs),
                        Deprecated = nodeContainerSpec.Deprecated,
                    }
                );
            }


            // Terminal structure specs
            specificationData.TerminalStructures = new List<TerminalStructureSpec>();

            foreach (var terminalStructureSpec in terminalStructureSpecs)
            {
                specificationData.TerminalStructures.Add(
                    new TerminalStructureSpec()
                    {
                        Name = terminalStructureSpec.Name,
                        Category = terminalStructureSpec.Category,
                        ShortName = terminalStructureSpec.ShortName,
                        Description = terminalStructureSpec.Description,
                        Manufacturers = GetManufacturesFromIds(manufacturerSpecs, terminalStructureSpec.ManufacturerRefs),
                        Deprecated = terminalStructureSpec.Deprecated,
                        Terminals = GetTerminals(terminalStructureSpec.TerminalTemplates)
                    }
                );
            }


            // Terminal equipment specs
            specificationData.TerminalEquipments = new List<TerminalEquipmentSpec>();

            foreach (var terminalEquipmentSpec in terminalEquipmentSpecs)
            {
                specificationData.TerminalEquipments.Add(
                    new TerminalEquipmentSpec()
                    {
                        //Id = terminalEquipment.Id,
                        Category = terminalEquipmentSpec.Category,
                        Name = terminalEquipmentSpec.Name,
                        ShortName = terminalEquipmentSpec.ShortName,
                        Description = terminalEquipmentSpec.Description,
                        Deprecated = terminalEquipmentSpec.Deprecated,
                        HeightInRackUnits = terminalEquipmentSpec.HeightInRackUnits,
                        Manufacturers = GetManufacturesFromIds(manufacturerSpecs, terminalEquipmentSpec.ManufacturerRefs),
                        IsRackEquipment = terminalEquipmentSpec.IsRackEquipment,
                        IsFixed = terminalEquipmentSpec.IsFixed,
                        IsAddressable = terminalEquipmentSpec.IsAddressable,
                        IsCustomerTermination = terminalEquipmentSpec.IsCustomerTermination,
                        IsLineTermination = terminalEquipmentSpec.IsLineTermination,
                        Structures = GetTerminalStructures(terminalEquipmentSpec.StructureTemplates, terminalStructureSpecs)
                    }
                );
            }


            // Eksport to file
            string json = JsonConvert.SerializeObject(specificationData, Formatting.Indented);
            
            File.WriteAllText(fileName, json);
        }

        public void ExportSpecificationList(string fileName)
        {
            var terminalEquipmentSpecs = _eventStore.Projections.Get<TerminalEquipmentSpecificationsProjection>().Specifications;

            var terminalStructureSpecs = _eventStore.Projections.Get<TerminalStructureSpecificationsProjection>().Specifications;

            var nodeContainerSpecs = _eventStore.Projections.Get<NodeContainerSpecificationsProjection>().Specifications;

            using StreamWriter csvFile = new(fileName);

            var csvHeader = "\"id\";\"type\";\"unique_spec_name\";\"short_name\";\"category\";\"height_in_rack_units\";\"deprecated\"";
            csvFile.WriteLine(csvHeader);

            foreach (var terminalEquipmentSpec in terminalEquipmentSpecs)
            {
                var csvLine = "\"" + terminalEquipmentSpec.Id + "\";\"TerminalEquipmentSpecification\";\"" + terminalEquipmentSpec.ShortName + "\";\"" + terminalEquipmentSpec.Name + "\";\"" + terminalEquipmentSpec.Category + "\";\"" + terminalEquipmentSpec.HeightInRackUnits + "\";\"" + terminalEquipmentSpec.Deprecated + "\"";
                csvFile.WriteLine(csvLine);
            }

            foreach (var terminalStructureSpec in terminalStructureSpecs)
            {
                var csvLine = "\"" + terminalStructureSpec.Id + "\";\"TerminalStructureSpecification\";\"" + terminalStructureSpec.ShortName + "\";\"" + terminalStructureSpec.Name + "\";\"" + terminalStructureSpec.Category + "\";\"" + 0 + "\";\"" + terminalStructureSpec.Deprecated + "\"";
                csvFile.WriteLine(csvLine);
            }

            foreach (var nodeContainerSpecification in nodeContainerSpecs)
            {
                var csvLine = "\"" + nodeContainerSpecification.Id + "\";\"NodeContainerSpecification\";\"" + nodeContainerSpecification.Name + "\";\"" + nodeContainerSpecification.Description + "\";\"" + nodeContainerSpecification.Category + "\";\"" + 0 + "\";\"" + nodeContainerSpecification.Deprecated + "\"";
                csvFile.WriteLine(csvLine);
            }


            csvFile.Close();
        }
        private List<TerminalTemplateSpec> GetTerminals(TerminalTemplate[] terminalTemplates)
        {
            if (terminalTemplates == null)
                return null;
            List<TerminalTemplateSpec> specs = new();

            foreach (var terminal in terminalTemplates)
            {
                specs.Add(
                    new TerminalTemplateSpec()
                    {
                        Name = terminal.Name,
                        Direction = terminal.Direction.ToString(),
                        IsPigtail = terminal.IsPigtail,
                        IsSplice = terminal.IsSplice,
                        ConnectorType = terminal.ConnectorType,
                        InternalConnectivityNode = terminal.InternalConnectivityNode
                    }
                );
            }

            return specs;
        }

        private List<TerminalStructureTemplateSpec> GetTerminalStructures(TerminalStructureTemplate[] structureTemplates, LookupCollection<TerminalStructureSpecification> terminalStructureSpecs)
        {
            if (structureTemplates == null)
                return null;

            List<TerminalStructureTemplateSpec> specs = new();

            foreach (var template in structureTemplates)
            {
                specs.Add(
                    new TerminalStructureTemplateSpec()
                    {
                        Position = template.Position,
                        RefName = terminalStructureSpecs[template.TerminalStructureSpecificationId].Name
                    }
                );
            }

            return specs;
        }

        private List<string> GetManufacturesFromIds(LookupCollection<UtilityGraphService.API.Model.UtilityNetwork.Manufacturer> manufactures, Guid[] manufacturerRefs)
        {
            if (manufacturerRefs == null)
                return null;

            List<string> manufacturerNames = new();

            foreach (var manufacturerRef in manufacturerRefs)
            {
                manufacturerNames.Add(manufactures[manufacturerRef].Name);
            }

            return manufacturerNames;
        }
    }
}
