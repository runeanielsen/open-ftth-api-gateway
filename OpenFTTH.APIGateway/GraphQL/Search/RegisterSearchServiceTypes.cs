using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.GraphQL.Search.Queries;
using OpenFTTH.APIGateway.GraphQL.Search.Types;

namespace OpenFTTH.APIGateway.GraphQL.Search
{
    public static class RegisterSearchServiceTypes
    {
        public static void Register(IServiceCollection services)
        {
            // Queries
            services.AddSingleton<SearchQueries>();

            // Search specific types
            services.AddSingleton<GlobalSearchHitType>();
        }
    }
}
