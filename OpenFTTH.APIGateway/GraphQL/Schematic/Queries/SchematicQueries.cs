using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Schematic.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Schematic.API.Queries;
using OpenFTTH.Schematic.Business.IO;
using System;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.Schematic.Queries
{
    public class SchematicQueries : ObjectGraphType
    {
        public SchematicQueries(ILogger<SchematicQueries> logger, IQueryDispatcher queryDispatcher)
        {
            Description = "GraphQL API for generating schematic diagrams";

            Field<DiagramType>(
                "buildDiagram",
                arguments: 
                new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNetworkElementId" },
                new QueryArgument<StringGraphType> { Name = "exportToGeoJsonFilename" }
                ),
                resolve: context =>
                {
                    if (!Guid.TryParse(context.GetArgument<string>("routeNetworkElementId"), out Guid routeNetworkElementId))
                    {
                        context.Errors.Add(new ExecutionError("Wrong value for guid"));
                        return null;
                    }

                    var getDiagramQueryResult = queryDispatcher.HandleAsync<GetDiagram, Result<GetDiagramResult>>(new GetDiagram(routeNetworkElementId)).Result;

                    if (getDiagramQueryResult.IsFailed)
                    {
                        context.Errors.Add(new ExecutionError(getDiagramQueryResult.Errors.First().Message));
                    }

                    // Export to geojson file (for checking in QGIS etc.) if such filename is specified
                    var jsonFilename = context.GetArgument<string>("exportToGeoJsonFilename");

                    if (jsonFilename != null)
                    {
                        var export = new GeoJsonExporter(getDiagramQueryResult.Value.Diagram);
                        export.Export(jsonFilename);
                    }

                    return getDiagramQueryResult.Value.Diagram;
                }
           );

        }

    }
}
