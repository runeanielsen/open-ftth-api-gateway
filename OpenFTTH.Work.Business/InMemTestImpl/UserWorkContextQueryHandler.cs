using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Queries;
using System.Threading.Tasks;

namespace OpenFTTH.Work.Business.InMemTestImpl
{
    public class UserWorkContextQueryHandler : 
        IQueryHandler<UserWorkContextQuery, UserWorkContextQueryResult>
    {
        private readonly InMemRepoImpl _data;

        public UserWorkContextQueryHandler(InMemRepoImpl data)
        {
            _data = data;
        }

        public Task<UserWorkContextQueryResult> HandleAsync(UserWorkContextQuery query)
        {
            return Task.FromResult < UserWorkContextQueryResult>(
                new UserWorkContextQueryResult(_data.GetUserWorkContext(query.UserName))
            );
        }
    }
}
