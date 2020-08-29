using DAX.EventProcessing.Dispatcher;
using DAX.EventProcessing.Dispatcher.Topos;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenFTTH.APIGateway.GraphQL.Schemas;
using OpenFTTH.APIGateway.Remote;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Queries;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Subscriptions;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Types;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.APIGateway.Workers;
using OpenFTTH.Events.RouteNetwork;
using OpenFTTH.Events.Geo;
using Serilog;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL.Mutations;
using OpenFTTH.APIGateway.GraphQL.Mutations;
using OpenFTTH.APIGateway.RouteNetwork.GraphQL;
using OpenFTTH.APIGateway.GraphQL.Queries;
using OpenFTTH.APIGateway.GraphQL.Subscriptions;
using OpenFTTH.APIGateway.GeographicalAreaUpdated.GraphQL.Types;

namespace OpenFTTH.APIGateway
{
    public class Startup
    {
        readonly string AllowedOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Logging
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, false)
               .AddEnvironmentVariables().Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

           
            services.AddLogging(loggingBuilder =>
                {
                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

                    loggingBuilder.AddSerilog(dispose: true);
                });
                       
            // GraphQL stuff
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddGraphQL(o => { o.ExposeExceptions = true; })
            .AddGraphTypes(ServiceLifetime.Singleton)
            .AddWebSockets()
            .AddDataLoader();

            // Settings
            services.Configure<KafkaSetting>(kafkaSettings =>
                            Configuration.GetSection("Kafka").Bind(kafkaSettings));

            services.Configure<RemoteServicesSetting>(remoteServiceSettings =>
                            Configuration.GetSection("RemoteServices").Bind(remoteServiceSettings));

            // Web stuff
            services.AddRazorPages();

            // GraphQL root schema
            services.AddSingleton<OpenFTTHSchema>();
            services.AddSingleton<OpenFTTHQueries>();
            services.AddSingleton<OpenFTTHMutations>();
            services.AddSingleton<OpenFTTHSubscriptions>();

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowedOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin();
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                  });
            });

            // Route network stuff
            services.AddSingleton<QueryServiceClient<RouteNetworkServiceQueries>>(x =>
                new QueryServiceClient<RouteNetworkServiceQueries>(
                    x.GetRequiredService<Microsoft.Extensions.Logging.ILogger<QueryServiceClient<RouteNetworkServiceQueries>>>(),
                    Configuration.GetSection("RemoteServices:RouteNetworkService").Value)
                );

            ConfigureRouteNetworkService.Register(services);

            services.AddHostedService<RouteNetworkEventConsumer>();
            services.AddSingleton<IToposTypedEventObservable<RouteNetworkEvent>, ToposTypedEventObservable<RouteNetworkEvent>>();

            // Geographical area updated
            services.AddHostedService<GeographicalAreaUpdatedEventConsumer>();
            services.AddSingleton<IToposTypedEventObservable<ObjectsWithinGeographicalAreaUpdated>, ToposTypedEventObservable<ObjectsWithinGeographicalAreaUpdated>>();
            services.AddSingleton<GeographicalAreaUpdatedEventSubscription>();
            services.AddSingleton<ObjectsWithinGeographicalAreaUpdatedType>();
            services.AddSingleton<EnvelopeType>();
            services.AddSingleton<IdChangeSetType>();
            services.AddSingleton<ChangeTypeEnumType>();

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

            app.UseCors(AllowedOrigins);

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
