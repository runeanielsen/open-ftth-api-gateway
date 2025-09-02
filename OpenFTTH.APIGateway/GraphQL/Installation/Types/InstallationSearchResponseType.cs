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

        Field("installationId", x => x.InstallationId, type: typeof(StringGraphType))
            .Description("Unique installation name maintained by the customer system.");

        Field("addressString", x => x.InstallationId, type: typeof(StringGraphType))
              .Description("Installation address");

        Field("additionalAddressInformation", x => x.InstallationId, type: typeof(StringGraphType))
               .Description("Installation additional address information");

        Field("distance", x => x.Distance, type: typeof(FloatGraphType))
            .Description("Distance from node");
    }
}
