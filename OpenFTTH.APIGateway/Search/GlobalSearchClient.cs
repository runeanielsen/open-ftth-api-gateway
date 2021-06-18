using OpenFTTH.APIGateway.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Typesense;

namespace OpenFTTH.APIGateway.Search
{
    public class GlobalSearchClient
    {
        private readonly ITypesenseClient _typesenseClient;

        public GlobalSearchClient(ITypesenseClient typesenseClient)
        {
            _typesenseClient = typesenseClient;
        }

        public Task<List<GlobalSearchHit>> Search(string searchString, int maxHits)
        {
            return SearchForAddresses(searchString, maxHits);
        }


        private async Task<List<GlobalSearchHit>> SearchForAddresses(string searchString, int maxHits)
        {
            var query = new SearchParameters
            {
                Text = searchString,
                QueryBy = "roadNameHouseNumber,postDistrictCode,postDistrictName,townName",
                PerPage = maxHits.ToString(),
                LimitHits = maxHits.ToString(),
                QueryByWeights = "5,3,3,2"
            };

            var searchResult = await _typesenseClient.Search<OfficialAccessAddress>("Addresses", query);

            List<GlobalSearchHit> result = new();

            foreach (var hit in searchResult.Hits)
            {
                var xEtrs = Double.Parse(hit.Document.EastCoordinate, CultureInfo.InvariantCulture);
                var yEtrs = Double.Parse(hit.Document.NorthCoordinate, CultureInfo.InvariantCulture);

                var wgs84Coord = UTM32WGS84Converter.ConvertFromUTM32NToWGS84(xEtrs, yEtrs);

                var globalHit = new GlobalSearchHit(hit.Document.Id, "accessAddress", GetAddressLabel(hit.Document), wgs84Coord[0], wgs84Coord[1], xEtrs, yEtrs);

                result.Add(globalHit);
            }

            return result;
        }

        private static string GetAddressLabel(OfficialAccessAddress address)
        {
            string result = address.RoadNameHouseNumber + ", ";

            if (address.TownName != null)
                result += address.TownName + ", ";

            result += address.PostDistrictCode + " " + address.PostDistrictName;

            return result;
        }
    }

    public record OfficialAccessAddress
    {
        public Guid Id { get; init; }
        public string Status { get; init; }
        public string RoadNameHouseNumber { get; init; }
        public string PostDistrictCode { get; init; }
        public string PostDistrictName { get; init; }
        public string EastCoordinate { get; init; }
        public string NorthCoordinate { get; init; }
        public string TownName { get; init; }
    }
}
