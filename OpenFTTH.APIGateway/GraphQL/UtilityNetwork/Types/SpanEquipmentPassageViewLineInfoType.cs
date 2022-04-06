using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.APIGateway.Util;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;
using System;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentPassageViewLineInfoType : ObjectGraphType<SpanEquipmentPassageViewLineInfo>
    {
        public SpanEquipmentPassageViewLineInfoType(ILogger<SpanEquipmentPassageViewLineInfoType> logger, UTM32WGS84Converter coordinateConverter)
        {
            Field(x => x.SpanSegmentId, type: typeof(IdGraphType)).Description("Span segment id");
            Field(x => x.From, type: typeof(StringGraphType)).Description("From");
            Field(x => x.To, type: typeof(StringGraphType)).Description("To");
            Field(x => x.ConduitId, type: typeof(StringGraphType)).Description("Span equipment name");
            Field(x => x.OuterConduitInfo, type: typeof(StringGraphType)).Description("Outer conduit info");
            Field(x => x.InnerConduitInfo, type: typeof(StringGraphType)).Description("Inner conduit info");

            Field<FloatGraphType>(
                name: "SegmentLength",
                description: "Segment length",
                resolve: context =>
                {
                    return Math.Round(context.Source.SegmentLength, 2);
                }
            );

            Field<FloatGraphType>(
               name: "CumulativeDistance",
               description: "Cumulative distance",
               resolve: context =>
               {
                   return Math.Round(context.Source.CumulativeDistance, 2);
               }
            );

            Field(x => x.RouteSegmentIds, type: typeof(ListGraphType<IdGraphType>)).Description("Route network segment ids of the span segment traversal");

            Field<ListGraphType<StringGraphType>>(
               name: "RouteSegmentGeometries",
               description: "Route network segment geometries of the span segment traversal",
               resolve: context =>
               {
                   return coordinateConverter.ConvertGeoJsonLineStringsToWgs84(context.Source.RouteSegmentGeometries);
               }
            );
        }
    }
}
