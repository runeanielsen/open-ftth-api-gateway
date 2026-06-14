using OpenFTTH.Results;
using OpenFTTH.CQRS;
using System;

namespace OpenFTTH.UtilityGraphService.API.Commands
{
    public record DisconnectSpanSegmentsFromTerminalsAtRouteNode : BaseCommand, ICommand<Result>
    {
        public Guid RouteNodeId { get; set; }
        public DisconnectSpanSegmentFromTerminalOperation[] Disconnects { get; set; }
        public DisconnectSpanSegmentsFromTerminalsAtRouteNode(Guid correlationId, UserContext userContext, Guid routeNodeId, DisconnectSpanSegmentFromTerminalOperation[] disconnects) : base(correlationId, userContext)
        {
            RouteNodeId = routeNodeId;
            Disconnects = disconnects;
        }
    }

    public record DisconnectSpanSegmentFromTerminalOperation
    {
        public Guid SpanSegmentId { get; set; }
        public Guid TerminalId { get; set; }

        public DisconnectSpanSegmentFromTerminalOperation(Guid spanSegmentId, Guid terminalId)
        {
            SpanSegmentId = spanSegmentId;
            TerminalId = terminalId;
        }
    }
}
