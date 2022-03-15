using FluentResults;
using GraphQL;
using GraphQL.Types;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Addresses.Types;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.GraphQL.RouteNetwork.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Events.Core.Infos;
using OpenFTTH.EventSourcing;
using OpenFTTH.UtilityGraphService.API.Commands;
using System;

namespace OpenFTTH.APIGateway.GraphQL.RouteNetwork.Mutations
{
    public class TerminalEquipmentMutations : ObjectGraphType
    {
        public TerminalEquipmentMutations(ICommandDispatcher commandDispatcher, IEventStore eventStore)
        {
            Description = "Terminal equipment mutations";

            Field<CommandResultType>(
             "updateProperties",
             description: "Mutation that can be used to change the terminal equipment specification,naming information",
             arguments: new QueryArguments(
                 new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "terminalEquipmentId" },
                 new QueryArgument<IdGraphType> { Name = "terminalEquipmentSpecificationId" },
                 new QueryArgument<IdGraphType> { Name = "manufacturerId" },
                 new QueryArgument<NamingInfoInputType> { Name = "namingInfo" },
                 new QueryArgument<AddressInfoInputType> { Name = "addressInfo" },
                 new QueryArgument<IdGraphType> { Name = "rackId" },
                 new QueryArgument<IntGraphType> { Name = "rackStartUnitPosition" }
             ),
             resolve: context =>
             {
                 var terminalEquipmentId = context.GetArgument<Guid>("terminalEquipmentId");

                 var correlationId = Guid.NewGuid();

                 var userContext = context.UserContext as GraphQLUserContext;
                 var userName = userContext.Username;

                 // TODO: Get from work manager
                 var workTaskId = Guid.Parse("54800ae5-13a5-4b03-8626-a63b66a25568");

                 var commandUserContext = new UserContext(userName, workTaskId);


                 var updateCmd = new UpdateTerminalEquipmentProperties(correlationId, commandUserContext, terminalEquipmentId: terminalEquipmentId)
                 {
                     SpecificationId = context.HasArgument("terminalEquipmentSpecificationId") ? context.GetArgument<Guid>("terminalEquipmentSpecificationId") : null,
                     ManufacturerId = context.HasArgument("manufacturerId") ? context.GetArgument<Guid>("manufacturerId") : null,
                     NamingInfo = context.HasArgument("namingInfo") ? context.GetArgument<NamingInfo>("namigInfo") : null,
                     AddressInfo = context.HasArgument("adressInfo") ? context.GetArgument<AddressInfo>("adressInfo") : null,
                     RackId = context.HasArgument("rackId") ? context.GetArgument<Guid>("rackId") : null,
                     StartUnitPosition = context.HasArgument("rackStartUnitPosition") ? context.GetArgument<int>("rackStartUnitPosition") : null,
                 };

                 var updateResult = commandDispatcher.HandleAsync<UpdateTerminalEquipmentProperties, Result>(updateCmd).Result;

                 return new CommandResult(updateResult);
             }
           );
        }
    }
}
