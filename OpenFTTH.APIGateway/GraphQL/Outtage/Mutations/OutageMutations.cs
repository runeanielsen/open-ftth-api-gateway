using FluentResults;
using GraphQL.Types;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.APIGateway.GraphQL.Core.Model;
using OpenFTTH.APIGateway.GraphQL.Schematic.Subscriptions;

namespace OpenFTTH.APIGateway.GraphQL.Outage.Mutations
{
    public class OutageMutations : ObjectGraphType
    {
        public OutageMutations(SchematicDiagramObserver schematicDiagramObserver)
        {
            Description = "Outage / trouble ticket mutations";

            Field<CommandResultType>(
              "sendTroubleTicket",
              description: "Send trouble ticket information to external systems",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "workOrderNumber" },
                  new QueryArgument<NonNullGraphType<ListGraphType<StringGraphType>>> { Name = "installationsIds" }
              ),
              resolve: context =>
              {
                  // Call external service responsible for publishing trouble ticket information to external systems

                  return new CommandResult(Result.Ok());
              }
            );
          
        }
    }
}
