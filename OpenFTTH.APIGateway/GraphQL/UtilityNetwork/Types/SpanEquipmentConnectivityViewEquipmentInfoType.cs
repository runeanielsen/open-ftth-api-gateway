using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.CQRS;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;
using OpenFTTH.UtilityGraphService.API.Queries;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentConnectivityViewEquipmentInfoType : ObjectGraphType<SpanEquipmentAZConnectivityViewEquipmentInfo>
    {
        public SpanEquipmentConnectivityViewEquipmentInfoType(ILogger<SpanEquipmentConnectivityViewEquipmentInfoType> logger, IQueryDispatcher queryDispatcher)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Master Resource Identifier UUID Property");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Name of the node structure");
            Field(x => x.Category, type: typeof(StringGraphType)).Description("Category");
            Field(x => x.Info, type: typeof(StringGraphType)).Description("Additional information (remark)");
            Field(x => x.SpecName, type: typeof(StringGraphType)).Description("Specification name");
            Field(x => x.Lines, type: typeof(ListGraphType<SpanEquipmentAZConnectivityViewLineInfoType>)).Description("Connectivity lines");

            FieldAsync<SpanEquipmentType>(
              name: "spanEquipment",
              description: "The span equipment",
              resolve: async context =>
              {
                  var spanEquipmentId = context.Source.Id;

                  // Get equipment information
                  var equipmentQueryResult = await queryDispatcher.HandleAsync<GetEquipmentDetails, FluentResults.Result<GetEquipmentDetailsResult>>(
                      new GetEquipmentDetails(new EquipmentIdList() { spanEquipmentId })
                  );

                  if (equipmentQueryResult.IsSuccess || equipmentQueryResult.Value.SpanEquipment != null && equipmentQueryResult.Value.SpanEquipment.ContainsKey(spanEquipmentId))
                  {
                      return equipmentQueryResult.Value.SpanEquipment[spanEquipmentId];
                  }

                  return null;
              }
           );

        }
    }
}
