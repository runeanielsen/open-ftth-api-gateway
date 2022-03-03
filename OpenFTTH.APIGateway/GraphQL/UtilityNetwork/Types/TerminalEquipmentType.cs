using FluentResults;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Addresses.Types;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class TerminalEquipmentType : ObjectGraphType<TerminalEquipment>
    {
        public TerminalEquipmentType(ILogger<TerminalEquipmentType> logger, IQueryDispatcher queryDispatcher)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Short name");
            Field(x => x.Description, type: typeof(StringGraphType)).Description("Long description");
            Field(x => x.AddressInfo, type: typeof(AddressInfoType)).Description("Address information such as access and unit address id");

            Field(x => x.SpecificationId, type: typeof(IdGraphType)).Description("Terminal equipment specification id");
            Field(x => x.ManufacturerId, type: typeof(IdGraphType)).Description("Terminal equipment manufacturer id");

            Field<SpanEquipmentSpecificationType>(
               name: "specification",
               description: "The specification used to create the span equipment",
               resolve: context =>
               {
                   var queryResult = queryDispatcher.HandleAsync<GetSpanEquipmentSpecifications, Result<LookupCollection<SpanEquipmentSpecification>>>(new GetSpanEquipmentSpecifications()).Result;

                   return queryResult.Value[context.Source.SpecificationId];
               }
            );

            Field<ManufacturerType>(
                name: "manufacturer",
                description: "The manufacturer of the span equipment",
                resolve: context =>
                {
                    if (context.Source.ManufacturerId == null || context.Source.ManufacturerId == Guid.Empty)
                        return null;

                    var queryResult = queryDispatcher.HandleAsync<GetManufacturer, Result<LookupCollection<Manufacturer>>>(new GetManufacturer()).Result;

                    return queryResult.Value[context.Source.ManufacturerId.Value];
                }
            );

        }
    }
}
