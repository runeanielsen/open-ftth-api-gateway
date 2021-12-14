using GraphQL.Types;
using Microsoft.Extensions.Logging;
using OpenFTTH.UtilityGraphService.API.Model.UtilityNetwork.Views;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class ConnectivityTraceViewType : ObjectGraphType<ConnectivityTraceView>
    {
        public ConnectivityTraceViewType(ILogger<ConnectivityTraceViewType> logger)
        {
            Field(x => x.CircuitName, type: typeof(StringGraphType)).Description("Circuit name");
            Field(x => x.Hops, type: typeof(ListGraphType<ConnectivityTraceViewHopInfoType>)).Description("The trace hops");
        }
    }
}
