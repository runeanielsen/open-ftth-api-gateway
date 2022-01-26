using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class SpanEquipmentAZConnectivityViewLineInfoType : ObjectGraphType<SpanEquipmentAZConnectivityViewLineInfo>
    {
        public SpanEquipmentAZConnectivityViewLineInfoType(ILogger<SpanEquipmentAZConnectivityViewLineInfoType> logger)
        {
            Field(x => x.SpanSegmentId, type: typeof(IdGraphType)).Description("Span segment id");
            Field(x => x.SequenceNumber, type: typeof(IntGraphType)).Description("Span structure sequence number");
            Field(x => x.Name, type: typeof(StringGraphType)).Description("Span structure name");
            Field(x => x.A, type: typeof(SpanEquipmentConnectivityViewEndInfoType)).Description("A info");
            Field(x => x.Z, type: typeof(SpanEquipmentConnectivityViewEndInfoType)).Description("Z info");
        }
    }
}
