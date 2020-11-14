using OpenFTTH.WorkService.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.WorkService.API.Mutations
{
    public class SetUserCurrentWorkTaskMutationResult : IMutationResult
    {
        public UserWorkContext UserWorkContext { get; set; }
    }
}
