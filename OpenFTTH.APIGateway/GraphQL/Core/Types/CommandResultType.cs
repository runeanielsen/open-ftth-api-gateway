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

            Field(x => x.IsSuccess, type: typeof(IdGraphType)).Name("IsSuccess").Description("True if the command was successfully processed on the server side.");
            Field(x => x.ErrorCode, type: typeof(IdGraphType)).Name("ErrorCode").Description("Error code. Please see error code lists defined in the underlying services called");
            Field(x => x.ErrorMessage, type: typeof(IdGraphType)).Name("ErrorMessage").Description("Error message");
        }
    }
}
