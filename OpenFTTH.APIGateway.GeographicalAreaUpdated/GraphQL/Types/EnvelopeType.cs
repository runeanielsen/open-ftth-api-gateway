using GraphQL.Types;
using OpenFTTH.Events.Geo;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFTTH.APIGateway.GeographicalAreaUpdated.GraphQL.Types
{
    public class EnvelopeType : ObjectGraphType<EnvelopeInfo>
    {
        public EnvelopeType()
        {
            Field(x => x.MinX, type: typeof(FloatGraphType)).Description("MinX");
            Field(x => x.MaxX, type: typeof(FloatGraphType)).Description("MaxX");
            Field(x => x.MinY, type: typeof(FloatGraphType)).Description("MinY");
            Field(x => x.MaxY, type: typeof(FloatGraphType)).Description("MaxY");
        }
    }
}
