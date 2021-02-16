using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Queries;
using OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork
{
    public static class RegisterUtilityNetworkTypes
    {
        public static void Register(IServiceCollection services)
        {
            // Queries
            services.AddSingleton<UtilityNetworkServiceQueries>();

            // Types
            services.AddSingleton<ManufacturerType>();
            services.AddSingleton<SpanEquipmentSpecificationType>();
        }
    }
}
