using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.GraphQL.Schematic.Queries;
using OpenFTTH.APIGateway.GraphQL.Schematic.Types;

namespace OpenFTTH.APIGateway.GraphQL.Schematic
{
    public static class RegisterSchematicTypes
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<SchematicQueries>();
            services.AddSingleton<DiagramType>();
            services.AddSingleton<DiagramObjectType>();
        }
    }
}
