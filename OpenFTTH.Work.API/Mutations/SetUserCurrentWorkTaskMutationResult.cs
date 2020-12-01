using OpenFTTH.CQRS;
using OpenFTTH.Work.API.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Mutations
{
    public class SetUserCurrentWorkTaskMutationResult : ICommand<SetUserCurrentWorkTaskMutationResult>
    {
        public SetUserCurrentWorkTaskMutationResult(UserWorkContext userWorkContext) => UserWorkContext = userWorkContext;
        public UserWorkContext UserWorkContext { get; }
    }
}
