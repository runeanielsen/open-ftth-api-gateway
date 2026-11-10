using OpenFTTH.Results;
using Microsoft.Extensions.Logging;
using OpenFTTH.CQRS;
using OpenFTTH.Schematic.API.Queries;
using OpenFTTH.Schematic.Business.SchematicBuilder;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.Schematic.Business.QueryHandler
{
    public class GetDiagramQueryHandler : IQueryHandler<GetDiagram, Result<GetDiagramResult>>
    {
        private readonly ILogger<GetDiagramQueryHandler> _logger;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetDiagramQueryHandler(ILogger<GetDiagramQueryHandler> logger, IQueryDispatcher queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<Result<GetDiagramResult>> HandleAsync(GetDiagram query)
        {
            var builder = new RouteNetworkElementDiagramBuilder(_logger, _queryDispatcher);

            var diagramBuildingResult = await builder.GetDiagram(query.RouteNetworkElementId).ConfigureAwait(false);

            if (diagramBuildingResult.IsFailed)
            {
                return Result.Fail<GetDiagramResult>(
                    new GetDiagramError(
                        GetDiagramErrorCodes.DIAGRAM_BUILDING_FAILED,
                        $"Error building diagram for route network element with id: {query.RouteNetworkElementId}"))
                    .WithError(diagramBuildingResult.Errors.First());
            }

            return Result.Ok(new GetDiagramResult(diagramBuildingResult.Value));
        }
    }
}
