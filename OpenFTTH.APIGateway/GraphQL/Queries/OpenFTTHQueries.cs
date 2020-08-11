using GraphQL.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Queries
{
    public class OpenFTTHQueries : ObjectGraphType
    {
        public OpenFTTHQueries(ILogger<OpenFTTHQueries> logger)
        {
            Description = "GraphQL API for querying Open FTTH";

            Field<StringGraphType>("apiVersion", resolve: context => VersionInfo.VersionString());

            /*
            Field<ListGraphType<RouteNodeType>>(
                "routeNodes",
                resolve: context =>
                {
                    return routeNetwork.GetAllRouteNodes();
                }
            );

            Field<RouteNodeType>(
                "routeNode",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }),
                resolve: context =>
                {
                    Guid id;
                    if (!Guid.TryParse(context.GetArgument<string>("id"), out id))
                    {
                        context.Errors.Add(new ExecutionError("Wrong value for guid"));
                        return null;
                    }

                    logger.LogDebug("Route node query: " + id);


                    return routeNetwork.GetRouteNodeInfo(id);
                }
            );


            Field<ListGraphType<RouteSegmentType>>(
                "routeSegments",
                resolve: context => {
                    return routeNetwork.GetAllRouteSegments();
                }
            );

            Field<RouteSegmentType>(
               "routeSegment",
               arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "Id" }),
               resolve: context =>
               {
                   Guid id;
                   if (!Guid.TryParse(context.GetArgument<string>("id"), out id))
                   {
                       context.Errors.Add(new ExecutionError("Wrong value for guid"));
                       return null;
                   }

                   logger.LogDebug("Route segment query: " + id);

                   return routeNetwork.GetRouteSegmentInfo(id);
               }
           );

            Field<ConduitServiceQuery>("conduitService", resolve: context => new { });
            Field<DiagramServiceQuery>("diagramService", resolve: context => new { });
            */
        }
    }
}
