using OpenFTTH.WorkService.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.WorkService.Queries
{
    public class ProjectsAndWorkTasksQueryResult : IQueryResult
    {
        public List<Project> Projects { get; set; }
    }
}
