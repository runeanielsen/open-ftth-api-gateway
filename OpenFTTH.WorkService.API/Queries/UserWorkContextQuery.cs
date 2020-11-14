using MediatR;
using System;

namespace OpenFTTH.WorkService.Queries
{
    public class UserWorkContextQuery : IQueryCommand
    {
        public string RequestName => typeof(UserWorkContextQuery).Name;

        public string UserName { get; }

        public UserWorkContextQuery(string userName)
        {
            UserName = userName;
        }
    }
}
