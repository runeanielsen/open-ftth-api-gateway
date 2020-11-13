using MediatR;
using System;

namespace OpenFTTH.WorkService.Queries
{
    public class ProjectsAndWorkTasksQuery : IQueryCommand
    {
        public string RequestName => typeof(ProjectsAndWorkTasksQuery).Name;

        public ProjectsAndWorkTasksQuery()
        {
        }
    }
}
