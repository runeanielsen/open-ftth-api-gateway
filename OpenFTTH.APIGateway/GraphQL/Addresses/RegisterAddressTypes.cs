using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.GraphQL.Addresses.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Addresses
{
    public static class RegisterAddressTypes
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<NearestAddressSearchHitType>();
            services.AddSingleton<AccessAddressType>();
        }
    }
}
