using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Addresses.Types
{
    public class NearestAddressSearchHit
    {
        public AccessAddressData AccessAddress { get; set; }
        public double Distance { get; set; }
    }
}
