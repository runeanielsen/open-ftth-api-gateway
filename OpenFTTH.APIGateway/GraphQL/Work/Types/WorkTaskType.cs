using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.CoreTypes;
using OpenFTTH.Work.API.Model;

namespace OpenFTTH.APIGateway.GraphQL.Work.Types
{
    public class WorkTaskType : ObjectGraphType<WorkTask>
    {
        public WorkTaskType(ILogger<WorkTaskType> logger)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name/description of work task");
            Field(x => x.SubtaskName, type: typeof(StringGraphType)).Description("Subtask name");
            Field(x => x.Type, type: typeof(StringGraphType)).Description("The type/category of the worktask - from work mgmt. system");
            Field(x => x.Status, type: typeof(StringGraphType)).Description("The status of the worktask - from work mgmt. system");
        }
    }


}
