using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.CoreTypes
{
    public static class RegisterCoreTypes
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<CommandResultType>();
            services.AddSingleton<GeometryType>();
        }
    }
}
