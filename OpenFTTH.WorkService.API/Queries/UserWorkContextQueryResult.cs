using OpenFTTH.WorkService.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.WorkService.Queries
{
    public class UserWorkContextQueryResult : IQueryResult
    {
        public UserWorkContext UserWorkContext { get; set; }
    }
}
