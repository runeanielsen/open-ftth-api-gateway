﻿using DAX.EventProcessing;
using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.APIGateway.IntegrationTests.TestData;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.EventSourcing.InMem;
using OpenFTTH.RouteNetwork.Business.RouteElements.StateHandling;
using System;
using System.Reflection;

namespace OpenFTTH.APIGateway.IntegrationTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Fake event producer
            services.AddSingleton<IExternalEventProducer, FakeExternalEventProducer>();

            // Route Network State and Repository
            services.AddSingleton<IRouteNetworkState, InMemRouteNetworkState>();
            services.AddSingleton<IRouteNetworkRepository, InMemRouteNetworkRepository>();

            // ES and CQRS
            services.AddSingleton<IEventStore, InMemEventStore>();

            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            var businessAssemblies = new Assembly[] { 
                AppDomain.CurrentDomain.Load("OpenFTTH.RouteNetwork.Business"),
                AppDomain.CurrentDomain.Load("OpenFTTH.UtilityGraphService.Business")
            };

            services.AddCQRS(businessAssemblies);

            services.AddProjections(businessAssemblies);

            // Test Route Network Data
            services.AddSingleton<ITestRouteNetworkData, TestRouteNetwork>();
        }
    }
}
