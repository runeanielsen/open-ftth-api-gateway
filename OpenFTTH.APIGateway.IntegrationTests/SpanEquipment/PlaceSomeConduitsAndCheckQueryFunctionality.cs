using FluentAssertions;
using FluentResults;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.IntegrationTests.TestData;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using OpenFTTH.Schematic.API.Queries;
using OpenFTTH.TestData;
using OpenFTTH.UtilityGraphService.API.Commands;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
using System.Linq;
using Xunit;
using Xunit.Extensions.Ordering;

namespace OpenFTTH.APIGateway.IntegrationTests.SpanEquipment
{
    /// <summary>
    /// Notice that this is an integration test, and that the tests are dependent on each other, and therefore must be run in order
    /// See https://raw.githubusercontent.com/DAXGRID/open-ftth-integration-tests/master/TestData/RouteNetworks/ForUnitTests/FiberRouteNetworkForUnitTesting-1.png
    /// for better understanding on what's going on in this test
    /// </summary>
    public class PlaceSomeConduitsAndCheckQueryFunctionality
    {
        private ILoggerFactory _loggerFactory;
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        private static TestSpecifications _specs;

        public PlaceSomeConduitsAndCheckQueryFunctionality(ILoggerFactory loggerFactory, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _loggerFactory = loggerFactory;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Fact, Order(1)]
        public void CreateSpanEquipmentSpecifications()
        {
            // Add conduit specifications
            _specs = new TestSpecifications(_loggerFactory, _commandDispatcher, _queryDispatcher).Run().Value;
        }


        [Fact, Order(10)]
        public async void CreateRouteNodeContainerInCC_1()
        {
            var nodeOfInterestId = Guid.NewGuid();
            var registerNodeOfInterestCommand = new RegisterNodeOfInterest(Guid.NewGuid(), new UserContext("test", Guid.Empty), nodeOfInterestId, TestRouteNetwork.CC_1);
            var registerNodeOfInterestCommandResult = _commandDispatcher.HandleAsync<RegisterNodeOfInterest, Result<RouteNetworkInterest>>(registerNodeOfInterestCommand).Result;

            var placeNodeContainerCommand = new PlaceNodeContainerInRouteNetwork(Guid.NewGuid(), new UserContext("test", Guid.Empty), Guid.NewGuid(), TestSpecifications.Conduit_Closure_Emtelle_Branch_Box, registerNodeOfInterestCommandResult.Value)
            {
                ManufacturerId = TestSpecifications.Manu_Emtelle
            };

            // Act
            var placeNodeContainerResult = await _commandDispatcher.HandleAsync<PlaceNodeContainerInRouteNetwork, Result>(placeNodeContainerCommand);

            var equipmentQueryResult = await _queryDispatcher.HandleAsync<GetEquipmentDetails, Result<GetEquipmentDetailsResult>>(
                new GetEquipmentDetails(new InterestIdList() { nodeOfInterestId })
            );

            // Assert
            placeNodeContainerResult.IsSuccess.Should().BeTrue();
            equipmentQueryResult.IsSuccess.Should().BeTrue();
            equipmentQueryResult.Value.NodeContainers.Count.Should().Be(1);
        }

        [Fact, Order(11)]
        public async void CheckThatDiagramIncludesNodeContainerInCC_1()
        {
            var sutRouteNetworkElement = TestRouteNetwork.CC_1;

            // Act
            var getDiagramQueryResult = await _queryDispatcher.HandleAsync<GetDiagram, Result<GetDiagramResult>>(new GetDiagram(sutRouteNetworkElement));

            // Assert
            getDiagramQueryResult.IsSuccess.Should().BeTrue();
            getDiagramQueryResult.Value.Diagram.DiagramObjects.Any(d => d.Style == "NodeContainer").Should().BeTrue();


        }



        [Fact, Order(20)]
        public async void Place5x10Conduit_from_HH_1_to_HH_10()
        {
            // Place a 5x10 multi conduit here:
            // (HH_1) <- (S2) -> (HH_2) <- (S4) -> (CC_1) <- (S13) -> (HH_10)
         
            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S2, TestRouteNetwork.S4, TestRouteNetwork.S13 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(Guid.NewGuid(), new UserContext("test", Guid.Empty), walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), new UserContext("test", Guid.Empty), Guid.NewGuid(), TestSpecifications.Multi_Ø40_5x10, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(21)]
        public async void Place10x10Conduit_from_HH_1_to_HH_10()
        {
            // Place a 1x10 multi conduit here:
            // (HH_1) <- (S2) -> (HH_2) <- (S4) -> (CC_1) <- (S13) -> (HH_10)

            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S2, TestRouteNetwork.S4, TestRouteNetwork.S13 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(Guid.NewGuid(), new UserContext("test", Guid.Empty), walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), new UserContext("test", Guid.Empty), Guid.NewGuid(), TestSpecifications.Multi_Ø50_10x10, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(22)]
        public async void PlaceØ40Flex_from_HH_2_to_FP_2()
        {
            // Place a Ø40 flex conduit here:
            // (HH_2) <- (S3) -> (FP_2)

            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S3 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(Guid.NewGuid(), new UserContext("test", Guid.Empty), walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), new UserContext("test", Guid.Empty), Guid.NewGuid(), TestSpecifications.Flex_Ø40_Red, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(23)]
        public async void PlaceØ40Flex_from_CC_1_to_SP_1()
        {
            // Place a Ø40 flex conduit here:
            // (CC_1) <- (S5) -> (SP_1)

            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S5 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(Guid.NewGuid(), new UserContext("test", Guid.Empty), walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), new UserContext("test", Guid.Empty), Guid.NewGuid(), TestSpecifications.Flex_Ø40_Red, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(50)]
        public async void QueryRouteNetworkDetailsOfCC_1()
        {
            var routeNetworkQuery = new GetRouteNetworkDetails(new RouteNetworkElementIdList() { TestRouteNetwork.CC_1 })
            {
                RelatedInterestFilter = RelatedInterestFilterOptions.ReferencesFromRouteElementAndInterestObjects
            };

            Result<GetRouteNetworkDetailsResult> routeNetworkQueryResult = await _queryDispatcher.HandleAsync<GetRouteNetworkDetails, Result<GetRouteNetworkDetailsResult>>(routeNetworkQuery);

            // Assert
            routeNetworkQueryResult.IsSuccess.Should().BeTrue();
            routeNetworkQueryResult.Value.RouteNetworkElements.Count.Should().Be(1);
            routeNetworkQueryResult.Value.Interests.Count.Should().Be(4);
            routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Length.Should().Be(4);
            routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Count(r => r.RelationKind == RouteNetworkInterestRelationKindEnum.PassThrough).Should().Be(2);
            routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Count(r => r.RelationKind == RouteNetworkInterestRelationKindEnum.Start).Should().Be(1);
        }


        [Fact, Order(100)]
        public async void QueryEquipmentDetailsOfCC_1()
        {
            var routeNetworkQuery = new GetRouteNetworkDetails(new RouteNetworkElementIdList() { TestRouteNetwork.CC_1 })
            {
                RelatedInterestFilter = RelatedInterestFilterOptions.ReferencesFromRouteElementOnly
            };

            // Act
            Result<GetRouteNetworkDetailsResult> routeNetworkQueryResult = await _queryDispatcher.HandleAsync<GetRouteNetworkDetails, Result<GetRouteNetworkDetailsResult>>(routeNetworkQuery);

            var interestIds = new InterestIdList();
            interestIds.AddRange(routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Select(r => r.RefId));

            var equipmentQueryResult = await _queryDispatcher.HandleAsync<GetEquipmentDetails, Result<GetEquipmentDetailsResult>>(
               new GetEquipmentDetails(interestIds)
            );

            // Assert
            equipmentQueryResult.IsSuccess.Should().BeTrue();
            equipmentQueryResult.Value.SpanEquipment.Count.Should().Be(3);
        }

    }
}
