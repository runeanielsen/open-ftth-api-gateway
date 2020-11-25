using OpenFTTH.Work.API.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Queries
{
    public class ProjectsAndWorkTasksQueryResult : IQueryResult
    {
        public ProjectsAndWorkTasksQueryResult(List<Project> projects) => Projects = projects;

        public List<Project> Projects { get; }
    }
}
