namespace OpenFTTH.APIGateway.GraphQL.Installation.Types;

public sealed record InstallationSearchResponse
{
    public string InstallationId { get; init; }
    public string DisplayAddress { get; init; }
    public string AdditionalAddressInformation { get; init; }
    public double Distance { get; init; }

    public InstallationSearchResponse(
        string installationId,
        string addressString,
        string additionalAddressInformation,
        double distance)
    {
        InstallationId = installationId;
        DisplayAddress = addressString;
        AdditionalAddressInformation = additionalAddressInformation;
        Distance = distance;
    }
}
