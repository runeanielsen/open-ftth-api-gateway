using GraphQL.Types;
using OpenFTTH.APIGateway.GraphQL.Core.Types;

namespace OpenFTTH.APIGateway.GraphQL.Installation.Types;

/// <summary>
/// Used for returning installation search result
/// </summary>
public sealed class InstallaionSearchResponseType : ObjectGraphType<InstallationSearchResponse>
{
    public InstallaionSearchResponseType()
    {
        Description = "Installation search result";

        Field("envelope", x => x.Envelope, type: typeof(EnvelopeType))
            .Description("The extent of the location.");

        Field("routeElementId", x => x.RouteElementId, type: typeof(IdGraphType))
            .Description("The route element id.");

        Field("coordinate", x => x.Coordinate, type: typeof(PointType))
            .Description("The coordinate for the point.");
    }
}
