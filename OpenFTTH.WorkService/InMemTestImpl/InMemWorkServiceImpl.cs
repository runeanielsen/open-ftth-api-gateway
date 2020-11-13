using OpenFTTH.WorkService.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.WorkService.InMemTestImpl
{
    public class InMemWorkServiceImpl : IWorkServiceAPI
    {
        private readonly InMemRepoImpl _data = new InMemRepoImpl();

        public IQueryResult Query(IQueryCommand queryCommand)
        {
            switch (queryCommand)
            {
                case ProjectsAndWorkTasksQuery query:
                    return (IQueryResult)Query(query);
            }

            throw new ArgumentException("No implementation found for: " + queryCommand.GetType().FullName);
        }

        private ProjectsAndWorkTasksQueryResult Query(ProjectsAndWorkTasksQuery query)
        {
            return new ProjectsAndWorkTasksQueryResult() { Projects = _data.Projects.Values.ToList() };
        }
    }
}
