using DAX.EventProcessing.Dispatcher;
using DAX.EventProcessing.Dispatcher.Topos;
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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Notifications.GeographicalAreaUpdated.Types;
using OpenFTTH.APIGateway.GraphQL.Root;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Queries;
using OpenFTTH.APIGateway.GraphQL.Work;
using OpenFTTH.APIGateway.Logging;
using OpenFTTH.APIGateway.Notifications.GeographicalAreaUpdated.Subscriptions;
using OpenFTTH.APIGateway.Remote;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.APIGateway.Workers;
using OpenFTTH.Events.Geo;
using OpenFTTH.Events.RouteNetwork;
using OpenFTTH.Work.API;
using OpenFTTH.WorkService.InMemTestImpl;
using Serilog;

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
            // To support event deserialization we need setup newtonsoft to this
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Converters.Add(new StringEnumConverter());
                settings.TypeNameHandling = TypeNameHandling.Auto;
                return settings;
            });

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

            services.AddGraphQL((options, provider) =>
            {
                options.EnableMetrics = false;
                var logger = provider.GetRequiredService<ILogger<Startup>>();
                options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occured", ctx.OriginalException.Message);
            })
            // Add required services for de/serialization
            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { }) // For .NET Core 3+
                                                                                       //.AddNewtonsoftJson(deserializerSettings => { }, serializerSettings => { }) // For everything else
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            .AddWebSockets() // Add required services for web socket support
            .AddDataLoader() // Add required services for DataLoader support
            .AddGraphTypes(typeof(Startup)); // Add all IGraphType implementors in assembly which Startup exists 


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

            // Core types
            RegisterCoreTypes.Register(services);

            // Route network service stuff
            services.AddSingleton<QueryServiceClient<RouteNetworkServiceQueries>>(x =>
                new QueryServiceClient<RouteNetworkServiceQueries>(
                    x.GetRequiredService<Microsoft.Extensions.Logging.ILogger<QueryServiceClient<RouteNetworkServiceQueries>>>(),
                    Configuration.GetSection("RemoteServices:RouteNetworkService").Value)
                );

            RegisterRouteNetworkServiceTypes.Register(services);

            services.AddHostedService<RouteNetworkEventConsumer>();
            services.AddSingleton<IToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent>, ToposTypedEventObservable<RouteNetworkEditOperationOccuredEvent>>();


            // Work service stuff
            RegisterWorkServiceTypes.Register(services);
            services.AddSingleton<IWorkServiceAPI, InMemWorkServiceImpl>();


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
            
            app.UseGraphQL<OpenFTTHSchema, GraphQLHttpMiddlewareWithLogs<OpenFTTHSchema>>("/graphql");

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
