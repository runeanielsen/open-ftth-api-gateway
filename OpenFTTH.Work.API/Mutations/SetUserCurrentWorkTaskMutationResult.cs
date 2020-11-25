using OpenFTTH.Work.API.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Mutations
{
    public class SetUserCurrentWorkTaskMutationResult : IMutationResult
    {
        public UserWorkContext UserWorkContext { get; set; }
    }
}
