using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.Specifications;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using System;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Mutations
{
    public class SpecificationMutations : ObjectGraphType
    {
        public SpecificationMutations(ILoggerFactory loggerFactory, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IEventStore eventStore)
        {
            Description = "Specification mutations";

            var logger = loggerFactory.CreateLogger(nameof(SpecificationMutations));

            Field<CommandResultType>(
                "importFromJsonString",
                description: "Import specifications from json string",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "json" }
                ),
                resolve: context =>
                {
                    try
                    {
                        var specificationJson = context.GetArgument<string>("json");

                        logger.LogInformation("Received the following JSON input: {Json} to be inserted as a specification.", specificationJson);

                        new SpecificationImporter(loggerFactory, commandDispatcher, queryDispatcher, eventStore).ImportFromJsonString(specificationJson);
                        return new CommandResult();
                    }
                    catch (Exception ex)
                    {
                        return new CommandResult(ex.Message);
                    }
                }
            );
        }
    }
}
