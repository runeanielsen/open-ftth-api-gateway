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

            FieldAsync<CommandResultType>(
                "importFromJsonString",
                description: "Import specifications from json string",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "json" }
                ),
                resolve: async context =>
                {
                    var specificationJson = context.GetArgument<string>("json");

                    var userContext = context.UserContext as GraphQLUserContext;
                    var userName = userContext.Username;

                    Guid correlationId = Guid.NewGuid();

                    try
                    {
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
