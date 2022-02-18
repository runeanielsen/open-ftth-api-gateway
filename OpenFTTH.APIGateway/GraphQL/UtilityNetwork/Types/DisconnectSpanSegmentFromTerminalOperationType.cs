using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Commands;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class DisconnectSpanSegmentFromTerminalOperationType : ObjectGraphType<DisconnectSpanSegmentFromTerminalOperation>
    {
        public DisconnectSpanSegmentFromTerminalOperationType(ILogger<DisconnectSpanSegmentFromTerminalOperationType> logger)
        {
            Field(x => x.SpanSegmentId, type: typeof(IdGraphType)).Description("Span segment id");
            Field(x => x.TerminalId, type: typeof(IdGraphType)).Description("Terminal id id");
        }
    }
}
