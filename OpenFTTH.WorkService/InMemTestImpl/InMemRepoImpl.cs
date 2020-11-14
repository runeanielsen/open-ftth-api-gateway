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

        private Dictionary<string, UserWorkContext> _userWorkContextByName = new Dictionary<string, UserWorkContext>();

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
                new WorkTask(Guid.Parse("523a168d-c14f-4fb9-b64a-b3adc2fee628"), "PON Privat 692345", new Point(566156, 6237162))
                {
                    AddressString = "Svalevej 5, Hinnerup",
                    CentralOfficeArea = "HINSON",
                    FlexPointArea = "F3410",
                    SplicePointArea = "F3416",
                    Technology = "PON",
                    InstallationId = "692345",
                    Status = "Til projektering",
                    WorkTaskType = "Privat kundetilslutning"
                });

            project2.AddWorkOrder(
                new WorkTask(Guid.Parse("08ce7a8f-4ad9-4b03-8386-86d3cebf408a"), "PON Privat 703443", new Point(566139, 6237187))
                {
                    AddressString = "Svalevej 7, Hinnerup",
                    CentralOfficeArea = "HINSON",
                    FlexPointArea = "F3410",
                    SplicePointArea = "F3416",
                    Technology = "PON",
                    InstallationId = "703443",
                    Status = "Til projektering",
                    WorkTaskType = "Privat kundetilslutning"
                });

            project2.AddWorkOrder(
                new WorkTask(Guid.Parse("efb70f56-7d0e-41a8-9a48-82a30aa48395"), "PON Privat 712323", new Point(566367, 6237104))
                {
                    AddressString = "Nørrevangen 54, Hinnerup",
                    CentralOfficeArea = "HINSON",
                    FlexPointArea = "F3410",
                    SplicePointArea = "F3413",
                    Technology = "PON",
                    InstallationId = "712323",
                    Status = "Til projektering",
                    WorkTaskType = "Privat kundetilslutning"
                });

            project2.AddWorkOrder(
              new WorkTask(Guid.Parse("086a5364-2cd4-481e-80d3-4f2d2025904d"), "PtP Erhverv 125377", new Point(565264, 6236742))
              {
                  AddressString = "Vestergade 82, Hinnerup",
                  CentralOfficeArea = "HINSON",
                  FlexPointArea = "F3570",
                  SplicePointArea = "F3575",
                  Technology = "PtP",
                  InstallationId = "125377",
                  Status = "Til projektering",
                  WorkTaskType = "Erhverv kundetilslutning"
              });

            Projects.Add(project2.MRID, project2);
        }

        public UserWorkContext GetUserWorkContext(string userName)
        {
            if (_userWorkContextByName.ContainsKey(userName))
                return _userWorkContextByName[userName];

            return null;
        }

        public UserWorkContext SetUserCurrentWorkTask(string userName, Guid currentWorkTaskId)
        {
            if (!_userWorkContextByName.ContainsKey(userName))
            {
                _userWorkContextByName[userName] = new UserWorkContext()
                {
                    UserName = userName,
                    CurrentWorkTask = currentWorkTaskId
                };
            }
            else
            {
                _userWorkContextByName[userName].CurrentWorkTask = currentWorkTaskId;
            }

            return _userWorkContextByName[userName];
        }
    }
}
