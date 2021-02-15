using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.GraphQL.Work.Types;
using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Queries;

namespace OpenFTTH.APIGateway.GraphQL.Work.Queries
{
    public class WorkServiceQueries : ObjectGraphType
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public WorkServiceQueries(ILogger<WorkServiceQueries> logger, IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;

            Description = "GraphQL API for querying work order related data";

            Field<ListGraphType<ProjectType>>(
                name: "projectsAndWorkTasks",
                description: "Retrieve all projects and underlying work tasks.",
                resolve: context =>
                {
                    var queryRequest = new ProjectsAndWorkTasksQuery();

                    var queryResult = this._queryDispatcher.HandleAsync<ProjectsAndWorkTasksQuery, ProjectsAndWorkTasksQueryResult>(queryRequest).Result;

                    return queryResult.Projects;
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

                    var queryResult = this._queryDispatcher.HandleAsync<UserWorkContextQuery, UserWorkContextQueryResult>(queryRequest).Result;

                    return queryResult.UserWorkContext;
                }
            );

        }

    }
}
