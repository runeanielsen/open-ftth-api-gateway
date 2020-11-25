using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.Work.API.Mutations
{
    public class SetUserCurrentWorkTaskMutation : IMutationCommand
    {
        public string RequestName => typeof(SetUserCurrentWorkTaskMutation).Name;

        public string UserName { get; }
        public Guid WorkTaskId { get; }

        public SetUserCurrentWorkTaskMutation(string userName, Guid workTaksId)
        {
            UserName = userName;
            WorkTaskId = workTaksId;
        }
    }
}
