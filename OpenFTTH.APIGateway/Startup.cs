using DAX.EventProcessing.Dispatcher;
using DAX.EventProcessing.Dispatcher.Topos;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Certificate;
using OpenFTTH.APIGateway.GraphQL.Schemas;
using OpenFTTH.APIGateway.Remote;
using OpenFTTH.APIGateway.RouteNetwork;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Queries;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Subscriptions;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.APIGateway.Workers;
using OpenFTTH.Events.RouteNetwork;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Logging
            Log.Logger = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .MinimumLevel.Verbose()
             .WriteTo.Console()
             .WriteTo.Debug()
             .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // GraphQL stuff
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddGraphQL(o => { o.ExposeExceptions = true; })
            .AddGraphTypes(ServiceLifetime.Scoped)
            .AddWebSockets()
            .AddDataLoader();

            // Settings
            services.Configure<KafkaSetting>(kafkaSettings =>
                            Configuration.GetSection("Kafka").Bind(kafkaSettings));

            services.Configure<RemoteServicesSetting>(remoteServiceSettings =>
                            Configuration.GetSection("RemoteServices").Bind(remoteServiceSettings));

            // Web stuff
            services.AddRazorPages();

            // Services used by the API gateways
            services.AddHostedService<RouteNetworkEventConsumer>();
            services.AddSingleton<IToposTypedEventObservable<RouteNetworkEvent>, ToposTypedEventObservable<RouteNetworkEvent>>();


            services.AddSingleton<QueryServiceClient<RouteNetworkServiceQueries>>(x =>
                new QueryServiceClient<RouteNetworkServiceQueries>(
                    x.GetRequiredService<Microsoft.Extensions.Logging.ILogger<QueryServiceClient<RouteNetworkServiceQueries>>>(), 
                    Configuration.GetSection("RemoteServices:RouteNetworkService").Value)
                );

            // GraphQL schema stuff
            services.AddScoped<OpenFTTHSchema>();

            services.AddSingleton<RouteNetworkEventSubscription>();
            services.AddSingleton<RouteNetworkEventType>();
            services.AddSingleton<RouteNetworkServiceQueries>();
            services.AddSingleton<RouteNodeType>();
            services.AddSingleton<NamingInfoType>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseWebSockets();
            app.UseGraphQLWebSockets<OpenFTTHSchema>("/graphql");
            app.UseGraphQL<OpenFTTHSchema>("/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground",
            });
            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphiQLPath = "/ui/graphiql",
                GraphQLEndPoint = "/graphql"
            });
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions
            {
                GraphQLEndPoint = "/graphql",
                Path = "/ui/voyager"
            });
        }
    }
}
