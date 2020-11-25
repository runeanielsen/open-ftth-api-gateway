using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Work.Types;
using OpenFTTH.Work.API;
using OpenFTTH.Work.API.Queries;

namespace OpenFTTH.APIGateway.GraphQL.Work.Queries
{
    public class WorkServiceQueries : ObjectGraphType
    {
        private readonly IWorkServiceAPI _workService;
        public WorkServiceQueries(ILogger<WorkServiceQueries> logger, IWorkServiceAPI workService)
        {
            _workService = workService;

            Description = "GraphQL API for querying work order related data";

            Field<ListGraphType<ProjectType>>(
                name: "projectsAndWorkTasks",
                description: "Retrieve all projects and underlying work tasks.",
                resolve: context =>
                {
                    var queryRequest = new ProjectsAndWorkTasksQuery();

                    return ((ProjectsAndWorkTasksQueryResult)_workService.Query(queryRequest)).Projects;
                }
            );

            Field<UserWorkContextType>(
                name: "userWorkContext",
                description: "Used to get work task status information specific to a particular user.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userName" }),
                resolve: context =>
                {
                    var userName = context.GetArgument<string>("userName");

                    var queryRequest = new UserWorkContextQuery(userName);

                    return ((UserWorkContextQueryResult)_workService.Query(queryRequest)).UserWorkContext;
                }
            );

        }

    }
}
