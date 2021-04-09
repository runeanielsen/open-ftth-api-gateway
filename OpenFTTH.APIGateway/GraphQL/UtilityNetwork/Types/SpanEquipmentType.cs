using CSharpFunctionalExtensions;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.CQRS;
using OpenFTTH.RouteNetwork.API.Model;
using OpenFTTH.RouteNetwork.API.Queries;
using OpenFTTH.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentType : ObjectGraphType<SpanEquipment>
    {
        public SpanEquipmentType(ILogger<SpanEquipmentType> logger, IQueryDispatcher queryDispatcher)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Short name");
            Field(x => x.Description, type: typeof(StringGraphType)).Description("Long description");
            Field(x => x.MarkingInfo, type: typeof(MarkingInfoType)).Description("Text and color marking information");

            Field(x => x.SpecificationId, type: typeof(IdGraphType)).Description("Span equipment specification id");
            Field(x => x.ManufacturerId, type: typeof(IdGraphType)).Description("Span equipment manufacturer id");

            Field<SpanEquipmentSpecificationType>(
               name: "specification",
               description: "The specification used to create the span equipment",
               resolve: context =>
               {
                   var queryResult = queryDispatcher.HandleAsync<GetSpanEquipmentSpecifications, Result<LookupCollection<SpanEquipmentSpecification>>>(new GetSpanEquipmentSpecifications()).Result;

                   return queryResult.Value[context.Source.SpecificationId];
               }
            );

            Field<ManufacturerType>(
                name: "manufacturer",
                description: "The manufacturer of the span equipment",
                resolve: context =>
                {
                    if (context.Source.ManufacturerId == null || context.Source.ManufacturerId == Guid.Empty)
                        return null;

                    var queryResult = queryDispatcher.HandleAsync<GetManufacturer, Result<LookupCollection<Manufacturer>>>(new GetManufacturer()).Result;

                    return queryResult.Value[context.Source.ManufacturerId.Value];
                }
            );

            Field<ListGraphType<IdGraphType>>(
                name: "routeSegmentIds",
                description: "The route network walk of the span equipment",
                resolve: context =>
                {
                    var queryResult = queryDispatcher.HandleAsync<GetRouteNetworkDetails, FluentResults.Result<GetRouteNetworkDetailsResult>>(
                        new GetRouteNetworkDetails(new InterestIdList() { context.Source.WalkOfInterestId })
                        {
                            RelatedInterestFilter = RelatedInterestFilterOptions.ReferencesFromRouteElementAndInterestObjects
                        }
                    ).Result;

                    if (queryResult.IsFailed)
                    {
                        throw new ApplicationException($"Got error querying interest information for span equipment: {context.Source.Id} ERROR: " + queryResult.Errors.First().Message);
                    }

                    if (queryResult.Value.Interests == null || !queryResult.Value.Interests.ContainsKey(context.Source.WalkOfInterestId))
                        throw new ApplicationException($"Got no info querying interest information for span equipment: {context.Source.Id}");

                    var routeNetworkElementIds = queryResult.Value.Interests[context.Source.WalkOfInterestId].RouteNetworkElementRefs;

                    List<Guid> segmentIds = new List<Guid>();

                    for (int i = 1; i < routeNetworkElementIds.Count; i += 2)
                        segmentIds.Add(routeNetworkElementIds[i]);

                    return segmentIds;
                }
            );

        }
    }
}
