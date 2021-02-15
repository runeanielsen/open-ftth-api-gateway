using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Queries;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Business.InMemTestImpl
{
    public class ProjectsAndWorkTasksQueryHandler : 
        IQueryHandler<ProjectsAndWorkTasksQuery, ProjectsAndWorkTasksQueryResult>
    {
        private readonly InMemRepoImpl _data;

        public ProjectsAndWorkTasksQueryHandler(InMemRepoImpl data)
        {
            _data = data;
        }

        public Task<ProjectsAndWorkTasksQueryResult> HandleAsync(ProjectsAndWorkTasksQuery query)
        {
            return Task.FromResult<ProjectsAndWorkTasksQueryResult>(
                new ProjectsAndWorkTasksQueryResult(_data.Projects.Values.ToList())
            );
        }
    }
}
