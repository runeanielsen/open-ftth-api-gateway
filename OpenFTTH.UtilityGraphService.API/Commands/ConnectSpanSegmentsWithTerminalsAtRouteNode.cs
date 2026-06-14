using OpenFTTH.Results;
using OpenFTTH.CQRS;
using System;

namespace OpenFTTH.UtilityGraphService.API.Commands
{
    public record ConnectSpanSegmentsWithTerminalsAtRouteNode : BaseCommand, ICommand<Result>
    {
        public Guid RouteNodeId { get; set; }
        public ConnectSpanSegmentToTerminalOperation[] Connects { get; set; }

        public ConnectSpanSegmentsWithTerminalsAtRouteNode(Guid correlationId, UserContext userContext, Guid routeNodeId, ConnectSpanSegmentToTerminalOperation[] connects) : base(correlationId, userContext)
        {
            RouteNodeId = routeNodeId;
            Connects = connects;
        }
    }

    public record ConnectSpanSegmentToTerminalOperation
    {
        public Guid SpanSegmentId { get; set; }
        public Guid TerminalId { get; set; }

        public ConnectSpanSegmentToTerminalOperation(Guid spanSegmentId, Guid terminalId)
        {
            SpanSegmentId = spanSegmentId;
            TerminalId = terminalId;
        }
    }
}
