using FluentResults;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.Util;
using OpenFTTH.CQRS;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
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

            Field<TerminalEquipmentSpecificationType>(
               name: "specification",
               description: "The specification used to create the terminal equipment",
               resolve: context =>
               {
                   var queryResult = queryDispatcher.HandleAsync<GetTerminalEquipmentSpecifications, Result<LookupCollection<TerminalEquipmentSpecification>>>(
                       new GetTerminalEquipmentSpecifications()).Result;

                   return queryResult.Value[context.Source.SpecificationId];
               }
            );

            Field<ManufacturerType>(
                name: "manufacturer",
                description: "The manufacturer of the terminal equipment",
                resolve: context =>
                {
                    if (context.Source.ManufacturerId == null || context.Source.ManufacturerId == Guid.Empty)
                        return null;

                    var queryResult = queryDispatcher.HandleAsync<GetManufacturer, Result<LookupCollection<Manufacturer>>>(new GetManufacturer()).Result;

                    return queryResult.Value[context.Source.ManufacturerId.Value];
                }
            );

            Field<SubrackPlacementInfoType>(
                name: "subrackPlacementInfo",
                description: "information about where in a rack the terminal equipment is placed",
                resolve: context =>
                {
                    var getNodeContainerResult = QueryHelper.GetNodeContainer(queryDispatcher, context.Source.NodeContainerId);

                    if (getNodeContainerResult.IsFailed)
                    {
                        return null;
                    }

                    var nodeContainer = getNodeContainerResult.Value;

                    // If the termnal equipment is placed within a rack in the node container, then return rack information
                    if (nodeContainer.Racks != null || nodeContainer.Racks.Any(r => r.SubrackMounts.Any(m => m.TerminalEquipmentId == context.Source.Id)))
                    {
                        var rack = nodeContainer.Racks.First(r => r.SubrackMounts.Any(m => m.TerminalEquipmentId == context.Source.Id));
                        var mount = rack.SubrackMounts.First(m => m.TerminalEquipmentId == context.Source.Id);

                        return new SubrackPlacementInfo(rack.Id, mount.Position, SubrackPlacmentMethod.BottomUp);
                    }
                    else
                    {
                        return null;
                    }
                }
            );

          
        }
    }
}
