using FluentResults;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Schematic.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Schematic.API.Queries;
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
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "routeNetworkElementId" }),
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

                    return getDiagramQueryResult.Value.Diagram;
                }
           );

        }

    }
}
