using OpenFTTH.Work.API.Queries;
using OpenFTTH.WorkService.InMemTestImpl;
using System;
using Xunit;

namespace OpenFTTH.WorkService.Tests
{
    public class QueryTests
    {
        [Fact]
        public void TestProjectsAndWorkTasksQuery()
        {
            var workService = new InMemWorkServiceImpl();

            var queryResult = workService.Query(new ProjectsAndWorkTasksQuery()) as ProjectsAndWorkTasksQueryResult;

            // Check that minimum 2 projects are returned
            Assert.True(queryResult.Projects.Count > 1);

            // Check that the two first projects has minumum 2 work tasks
            Assert.True(queryResult.Projects[0].WorkTasks.Count > 1);
            Assert.True(queryResult.Projects[1].WorkTasks.Count > 0);

        }
    }
}
