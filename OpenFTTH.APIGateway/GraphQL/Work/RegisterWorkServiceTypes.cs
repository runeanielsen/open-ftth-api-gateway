using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.GraphQL.Work.Mutations;
using OpenFTTH.APIGateway.GraphQL.Work.Queries;
using OpenFTTH.APIGateway.GraphQL.Work.Types;

namespace OpenFTTH.APIGateway.GraphQL.Work
{
    public static class RegisterWorkServiceTypes
    {
        public static void Register(IServiceCollection services)
        {
            // Queries
            services.AddSingleton<WorkServiceQueries>();

            // Mutations
            services.AddSingleton<UserWorkContextMutations>();

            // Work specific types
            services.AddSingleton<WorkTaskType>();
            services.AddSingleton<UserWorkContextType>();
        }
    }
}
