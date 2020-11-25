using OpenFTTH.Work.API.Mutations;
using OpenFTTH.Work.API.Queries;
using OpenFTTH.WorkService.InMemTestImpl;
using System;
using Xunit;

namespace OpenFTTH.WorkService.Tests
{
    public class MutationTests
    {
        [Fact]
        public void SetUserCurrentWorkTask_ThatDontExists_MustThrowException()
        {
            var workService = new InMemWorkServiceImpl();

            // Some work task id that don't exists
            Guid workTaskId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() => workService.Mutate(new SetUserCurrentWorkTaskMutation("hans", workTaskId)) as SetUserCurrentWorkTaskMutationResult);
        }

        [Fact]
        public void SetUserCurrentWorkTask_ThatExists_QueryMustReturnWorkTask()
        {
            var workService = new InMemWorkServiceImpl();

            Guid workTaskId = Guid.Parse("08ce7a8f-4ad9-4b03-8386-86d3cebf408a");

            var mutationResult = workService.Mutate(new SetUserCurrentWorkTaskMutation("hans", workTaskId)) as SetUserCurrentWorkTaskMutationResult;

            var queryResult = workService.Query(new UserWorkContextQuery("hans")) as UserWorkContextQueryResult;

            Assert.Equal(workTaskId, queryResult.UserWorkContext.CurrentWorkTask.MRID);

            Assert.NotNull(queryResult.UserWorkContext.CurrentWorkTask.Project);
        }
    }
}
