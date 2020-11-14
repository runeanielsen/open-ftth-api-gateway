using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.WorkService.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.Work.GraphQL.Types
{
    public class UserWorkContextType : ObjectGraphType<UserWorkContext>
    {
        public UserWorkContextType(ILogger<UserWorkContextType> logger)
        {
            Field(x => x.UserName, type: typeof(StringGraphType)).Description("User name/id");
            Field(x => x.CurrentWorkTask, type: typeof(StringGraphType)).Description("CurrentWorkTask");
        }
    }
}
