using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Queries
{
    public class UserWorkContextQueryResult : IQuery<UserWorkContextQueryResult>
    {
        public UserWorkContextQueryResult(UserWorkContext userWorkContext) => UserWorkContext = userWorkContext;
        
        public UserWorkContext UserWorkContext { get; }
    }
}
