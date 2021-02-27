using GraphQL.Types;
using NetTopologySuite.Geometries;

namespace OpenFTTH.APIGateway.GraphQL.Core.Types
{
    public class EnvelopeType : ObjectGraphType<Envelope>
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
