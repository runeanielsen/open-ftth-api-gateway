using FluentAssertions;
using FluentResults;
using OpenFTTH.APIGateway.IntegrationTests.TestData;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Commands;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
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
        private ICommandDispatcher _commandDispatcher;
        private IQueryDispatcher _queryDispatcher;

        private static TestSpecifications _specs;

        public PlaceSomeConduitsAndCheckQueryFunctionality(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Fact, Order(1)]
        public void CreateSpanEquipmentSpecifications()
        {
            // Add conduit specifications
            _specs = new TestSpecifications(_commandDispatcher, _queryDispatcher).Run();
        }


        [Fact, Order(2)]
        public async void Place5x10Conduit_from_HH_1_to_HH_10()
        {
            // Place a 5x10 multi conduit here:
            // (HH_1) <- (S2) -> (HH_2) <- (S4) -> (CC_1) <- (S13) -> (HH_10)
         
            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S2, TestRouteNetwork.S4, TestRouteNetwork.S13 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), TestSpecifications.Multi_Ø40_5x10, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(2)]
        public async void Place10x10Conduit_from_HH_1_to_HH_10()
        {
            // Place a 1x10 multi conduit here:
            // (HH_1) <- (S2) -> (HH_2) <- (S4) -> (CC_1) <- (S13) -> (HH_10)

            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S2, TestRouteNetwork.S4, TestRouteNetwork.S13 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), TestSpecifications.Multi_Ø50_10x10, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(3)]
        public async void PlaceØ40Flex_from_HH_2_to_FP_2()
        {
            // Place a Ø40 flex conduit here:
            // (HH_2) <- (S3) -> (FP_2)

            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S3 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), TestSpecifications.Flex_Ø40_Red, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(4)]
        public async void PlaceØ40Flex_from_CC_1_to_SP_1()
        {
            // Place a Ø40 flex conduit here:
            // (CC_1) <- (S5) -> (SP_1)

            // First register the walk in the route network where we want to place the conduit
            var walkOfInterestId = Guid.NewGuid();
            var walk = new RouteNetworkElementIdList() { TestRouteNetwork.S5 };
            var registerWalkOfInterestCommand = new RegisterWalkOfInterest(walkOfInterestId, walk);
            var registerWalkOfInterestCommandResult = await _commandDispatcher.HandleAsync<RegisterWalkOfInterest, Result<RouteNetworkInterest>>(registerWalkOfInterestCommand);

            // Now place the conduit in walk
            var placeSpanEquipmentCommand = new PlaceSpanEquipmentInRouteNetwork(Guid.NewGuid(), TestSpecifications.Flex_Ø40_Red, registerWalkOfInterestCommandResult.Value);
            var placeSpanEquipmentResult = await _commandDispatcher.HandleAsync<PlaceSpanEquipmentInRouteNetwork, Result>(placeSpanEquipmentCommand);

            // Assert
            registerWalkOfInterestCommandResult.IsSuccess.Should().BeTrue();
            placeSpanEquipmentResult.IsSuccess.Should().BeTrue();
        }

        [Fact, Order(10)]
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
            routeNetworkQueryResult.Value.Interests.Count.Should().Be(3);
            routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Length.Should().Be(3);
            routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Count(r => r.RelationKind == RouteNetworkInterestRelationKindEnum.PassThrough).Should().Be(2);
            routeNetworkQueryResult.Value.RouteNetworkElements[TestRouteNetwork.CC_1].InterestRelations.Count(r => r.RelationKind == RouteNetworkInterestRelationKindEnum.Start).Should().Be(1);
        }


        [Fact, Order(11)]
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
