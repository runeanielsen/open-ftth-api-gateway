using GraphQL;
using GraphQL.Types;
using Marten.Events;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.Reporting;
using OpenFTTH.APIGateway.Search;
using OpenFTTH.APIGateway.Settings;
using OpenFTTH.APIGateway.Util;
using OpenFTTH.CQRS;
using OpenFTTH.EventSourcing;
using OpenFTTH.RouteNetwork.Business.RouteElements.StateHandling;
using Topos.Logging;
using Typesense;

namespace OpenFTTH.APIGateway.GraphQL.Search.Queries
{
    public class ReportingQueries : ObjectGraphType
    {
        public ReportingQueries(ILogger<SearchQueries> logger, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, EventSourcing.IEventStore eventStore, IRouteNetworkState routeNetworkState)
        {
            Description = "GraphQL API for reporting operations";

            FieldAsync<ListGraphType<StringGraphType>>(
                name: "customerTerminationTrace",
                description: "Do upstream trace on all customer terminations in the network",
                resolve: async (context) =>
                {
                    var traceReport = new CustomerTerminationReport(loggerFactory.CreateLogger<CustomerTerminationReport>(), eventStore, routeNetworkState);

                    return traceReport.TraceAllCustomerTerminations();
                }
            );
        }
    }
}
