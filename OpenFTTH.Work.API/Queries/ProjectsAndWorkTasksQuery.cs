using MediatR;
using System;

namespace OpenFTTH.Work.API.Queries
{
    public class ProjectsAndWorkTasksQuery : IQueryCommand
    {
        public string RequestName => typeof(ProjectsAndWorkTasksQuery).Name;

        public ProjectsAndWorkTasksQuery()
        {
        }
    }
}
