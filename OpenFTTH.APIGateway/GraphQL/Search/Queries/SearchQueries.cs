using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Search.Types;
using OpenFTTH.APIGateway.GraphQL.Work.Types;
using OpenFTTH.APIGateway.Search;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Queries;
using Typesense;

namespace OpenFTTH.APIGateway.GraphQL.Search.Queries
{
    public class SearchQueries : ObjectGraphType
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public SearchQueries(ILogger<SearchQueries> logger, IQueryDispatcher queryDispatcher, ITypesenseClient typesenseClient)
        {
            _queryDispatcher = queryDispatcher;

            Description = "GraphQL API for search operations";

            Field<ListGraphType<GlobalSearchHitType>>(
                name: "globalSearch",
                description: "Search for route network node, adresses etc.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "searchString" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "pageSize" }
                ),
                resolve: context =>
                {
                    var searchString = context.GetArgument<string>("searchString");
                    var pageSize = context.GetArgument<int>("pageSize");

                    var searchClient = new GlobalSearchClient(typesenseClient);

                    return searchClient.Search(searchString, pageSize);
                }
            );
        }
    }
}
