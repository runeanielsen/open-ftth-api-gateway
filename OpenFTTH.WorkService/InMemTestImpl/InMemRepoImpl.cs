using NetTopologySuite.Geometries;
using OpenFTTH.WorkService.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.WorkService.InMemTestImpl
{
    public class InMemRepoImpl
    {
        public Dictionary<Guid, Project> Projects { get; set; }

        public InMemRepoImpl()
        {
            // throw in some test data
            Projects = new Dictionary<Guid, Project>();

            var project1 = new Project(Guid.Parse("55783514-4481-4f62-a321-e025c62741ac"), "Dataopretning");

            project1.AddWorkOrder(
                new WorkTask(Guid.Parse("dd07c615-0497-48f0-825f-ccac0c7596d0"), "Dataopretning af tracénet"));

            project1.AddWorkOrder(
                new WorkTask(Guid.Parse("4640df62-f4a9-4490-9400-f7ef88b64bfa"), "Dataopretning af Bentley rør-konnektivitet"));

            Projects.Add(project1.MRID, project1);


            var project2 = new Project(Guid.Parse("68929c17-400d-4379-9794-137879c5959a"), "Kundetilslutning");

            project2.AddWorkOrder(
                new WorkTask(Guid.Parse("523a168d-c14f-4fb9-b64a-b3adc2fee628"), "Svalevej 5, Hinnerup", new Point(566156, 6237162)));

            project2.AddWorkOrder(
                new WorkTask(Guid.Parse("08ce7a8f-4ad9-4b03-8386-86d3cebf408a"), "Svalevej 7, Hinnerup", new Point(566139, 6237187)));

            project2.AddWorkOrder(
                new WorkTask(Guid.Parse("efb70f56-7d0e-41a8-9a48-82a30aa48395"), "Nørrevangen 54, Hinnerup", new Point(566367, 6237104)));

            Projects.Add(project2.MRID, project2);
        }
    }
}
