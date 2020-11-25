using OpenFTTH.Work.API.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Queries
{
    public class ProjectsAndWorkTasksQueryResult : IQueryResult
    {
        public List<Project> Projects { get; set; }
    }
}
