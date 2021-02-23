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
                ErrorCode = result.Errors.First().Metadata["ErrorCode"].ToString();
                ErrorMessage = result.Errors.First().Message;
            }
            else
            {
                IsSuccess = true;
            }
        }
    }
}
