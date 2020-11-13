using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.Work.GraphQL.Types;
using OpenFTTH.WorkService;
using OpenFTTH.WorkService.Queries;

namespace OpenFTTH.APIGateway.Work.GraphQL.Queries
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

        }

    }
}
