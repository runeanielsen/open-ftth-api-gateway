using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.GraphQL.Addresses.Types
{
    public class NearestAddressSearchHitType : ObjectGraphType<NearestAddressSearchHit>
    {
        public NearestAddressSearchHitType()
        {
            Field(x => x.AccessAddress, type: typeof(AccessAddressType)).Description("Access Address Information");
            Field(x => x.Distance, type: typeof(FloatGraphType)).Description("Distance to Access Address");
        }
    }
}
