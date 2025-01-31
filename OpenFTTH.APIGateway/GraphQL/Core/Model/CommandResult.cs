using FluentResults;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.Core.Model
{
    public class CommandResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public CommandResult(Result result)
        {
            if (result.IsFailed)
            {
                IsSuccess = false;

                var error = result.Errors.First();

                if (error.Metadata != null && error.HasMetadataKey("ErrorCode"))
                    ErrorCode = result.Errors.First().Metadata["ErrorCode"].ToString();
                else
                    ErrorCode = "ERROR";

                ErrorMessage = result.Errors.First().Message;
            }
            else
            {
                IsSuccess = true;
            }
        }

        public CommandResult()
        {
           IsSuccess = true;
        }
        public CommandResult(string errorMsg)
        {
            IsSuccess = false;
            ErrorMessage = errorMsg;
        }
    }
}
