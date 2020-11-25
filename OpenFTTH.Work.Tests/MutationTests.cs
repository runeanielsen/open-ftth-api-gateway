using OpenFTTH.Work.API.Mutations;
using OpenFTTH.Work.API.Queries;
using OpenFTTH.Work.Business.InMemTestImpl;
using System;
using Xunit;

namespace OpenFTTH.WorkService.Tests
{
    public class MutationTests
    {
        [Fact]
        public void SetUserCurrentWorkTask_ThatDontExists_MustThrowException()
        {
            var inMemRepo = new InMemRepoImpl();

            var inMemCommandHandler = new InMemCommandHandler(inMemRepo);

            // Some work task id that don't exists
            Guid workTaskId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() => inMemCommandHandler.HandleAsync(new SetUserCurrentWorkTaskMutation("hans", workTaskId)).Result);
        }

        [Fact]
        public void SetUserCurrentWorkTask_ThatExists_QueryMustReturnWorkTask()
        {
            var repo = new InMemRepoImpl();

            var commandHandler = new InMemCommandHandler(repo);

            var queryHandler = new InMemQueryHandler(repo);

            Guid workTaskId = Guid.Parse("08ce7a8f-4ad9-4b03-8386-86d3cebf408a");

            var mutationResult = commandHandler.HandleAsync(new SetUserCurrentWorkTaskMutation("hans", workTaskId)).Result;

            var queryResult = queryHandler.HandleAsync(new UserWorkContextQuery("hans")).Result;

            Assert.Equal(workTaskId, queryResult.UserWorkContext.CurrentWorkTask.MRID);

            Assert.NotNull(queryResult.UserWorkContext.CurrentWorkTask.Project);
        }
    }
}
