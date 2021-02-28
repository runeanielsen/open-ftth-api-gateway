using GraphQL.DataLoader;
using GraphQL.Types;
using OpenFTTH.APIGateway.GraphQL.Core.Model;

namespace OpenFTTH.APIGateway.CoreTypes
{
    public class CommandResultType : ObjectGraphType<CommandResult>
    {
        public CommandResultType(IDataLoaderContextAccessor dataLoader)
        {
            Description = "Error code and message";

            Field(x => x.IsSuccess, type: typeof(BooleanGraphType)).Name("IsSuccess").Description("True if the command was successfully processed on the server side.");
            Field(x => x.ErrorCode, type: typeof(StringGraphType)).Name("ErrorCode").Description("Error code. Please see error code lists defined in the underlying services called");
            Field(x => x.ErrorMessage, type: typeof(StringGraphType)).Name("ErrorMessage").Description("Error message");
        }
    }
}
