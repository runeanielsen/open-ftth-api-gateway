using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.Work.GraphQL.Queries;
using OpenFTTH.APIGateway.Work.GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.Work.GraphQL
{
    public static class RegisterWorkServiceTypes
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<WorkServiceQueries>();

            // Work specific types
            services.AddSingleton<ProjectType>();
            services.AddSingleton<WorkTaskType>();
        }
    }
}
