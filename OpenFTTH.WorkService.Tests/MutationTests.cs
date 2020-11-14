using OpenFTTH.WorkService.API.Mutations;
using OpenFTTH.WorkService.InMemTestImpl;
using OpenFTTH.WorkService.Queries;
using System;
using Xunit;

namespace OpenFTTH.WorkService.Tests
{
    public class MutationTests
    {
        [Fact]
        public void TestSetUserCurrentWorkTask()
        {
            var workService = new InMemWorkServiceImpl();

            Guid workTaskId = Guid.NewGuid();

            var mutationResult = workService.Mutate(new SetUserCurrentWorkTaskMutation("hans", workTaskId)) as SetUserCurrentWorkTaskMutationResult;

            var queryResult = workService.Query(new UserWorkContextQuery("hans")) as UserWorkContextQueryResult;

            Assert.Equal(workTaskId, queryResult.UserWorkContext.CurrentWorkTask);
        }
    }
}
