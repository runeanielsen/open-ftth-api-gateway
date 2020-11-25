using MediatR;
using OpenFTTH.CQRS;
using System;

namespace OpenFTTH.Work.API.Queries
{
    public class ProjectsAndWorkTasksQuery : IQuery<ProjectsAndWorkTasksQueryResult>
    {
        public string RequestName => typeof(ProjectsAndWorkTasksQuery).Name;

        public ProjectsAndWorkTasksQuery()
        {
        }
    }
}
