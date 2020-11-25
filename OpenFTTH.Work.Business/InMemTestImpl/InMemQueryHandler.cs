using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Business.InMemTestImpl
{
    public class InMemQueryHandler : 
        IQueryHandler<ProjectsAndWorkTasksQuery, ProjectsAndWorkTasksQueryResult>,
        IQueryHandler<UserWorkContextQuery, UserWorkContextQueryResult>
    {
        private readonly InMemRepoImpl _data;

        public InMemQueryHandler(InMemRepoImpl data)
        {
            _data = data;
        }

        public Task<ProjectsAndWorkTasksQueryResult> HandleAsync(ProjectsAndWorkTasksQuery query)
        {
            return Task.FromResult<ProjectsAndWorkTasksQueryResult>(
                new ProjectsAndWorkTasksQueryResult(_data.Projects.Values.ToList())
            );
        }

        public Task<UserWorkContextQueryResult> HandleAsync(UserWorkContextQuery query)
        {
            return Task.FromResult < UserWorkContextQueryResult>(
                new UserWorkContextQueryResult(_data.GetUserWorkContext(query.UserName))
            );
        }
    }
}
