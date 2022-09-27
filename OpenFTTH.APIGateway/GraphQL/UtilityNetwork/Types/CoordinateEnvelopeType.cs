using GraphQL.Types;

namespace OpenFTTH.APIGateway.GraphQL.UtilityNetwork.Types
{
    public class CoordinateEnvelopeType : ObjectGraphType<CoordinateEnvelope>
    {
        public CoordinateEnvelopeType()
        {
            Field(x => x.WGS84MinX, type: typeof(FloatGraphType)).Description("MinX in WGS89");
            Field(x => x.WGS84MinY, type: typeof(FloatGraphType)).Description("MinY in WGS89");
            Field(x => x.WGS84MaxX, type: typeof(FloatGraphType)).Description("MaxX in WGS89");
            Field(x => x.WGS84MaxY, type: typeof(FloatGraphType)).Description("MaxY in WGS89");

            Field(x => x.ETRS89MinX, type: typeof(FloatGraphType)).Description("MinX in ETRS89");
            Field(x => x.ETRS89MinY, type: typeof(FloatGraphType)).Description("MinY in ETRS89");
            Field(x => x.ETRS89MaxX, type: typeof(FloatGraphType)).Description("MaxX in ETRS89");
            Field(x => x.ETRS89MaxY, type: typeof(FloatGraphType)).Description("MaxY in ETRS89");
        }
    }
}
