﻿using CSharpFunctionalExtensions;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Queries;


namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Queries
{
    public class UtilityNetworkServiceQueries : ObjectGraphType
    {
        public UtilityNetworkServiceQueries(ILogger<UtilityNetworkServiceQueries> logger, IQueryDispatcher queryDispatcher)
        {
            Description = "GraphQL API for querying data owned by utility network service";

            Field<ListGraphType<ManufacturerType>>(
                name: "manufacturers",
                description: "Retrieve all manufacturers.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetManufacturer, Result<LookupCollection<Manufacturer>>>(new GetManufacturer()).Result;

                    return queryResult.Value;
                }
            );

            Field<ListGraphType<SpanEquipmentSpecificationType>>(
                name: "spanEquipmentSpecifications",
                description: "Retrieve all span equipment specifications.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetSpanEquipmentSpecifications, Result<LookupCollection<SpanEquipmentSpecification>>>(new GetSpanEquipmentSpecifications()).Result;

                    return queryResult.Value;
                }
            );

            Field<ListGraphType<NodeContainerSpecificationType>>(
                name: "nodeContainerSpecifications",
                description: "Retrieve all node container specifications.",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetNodeContainerSpecifications, Result<LookupCollection<NodeContainerSpecification>>>(new GetNodeContainerSpecifications()).Result;

                    return queryResult.Value;
                }
            );
        }
    }
}
