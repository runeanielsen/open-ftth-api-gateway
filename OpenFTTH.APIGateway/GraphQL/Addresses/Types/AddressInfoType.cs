using GraphQL.Types;
using OpenFTTH.Events.Core.Infos;

namespace OpenFTTH.APIGateway.GraphQL.Addresses.Types
{
    public class AddressInfoType : ObjectGraphType<AddressInfo>
    {
        public AddressInfoType()
        {
            Field(x => x.AccessAddressId, type: typeof(IdGraphType)).Description("Internal or external access address id");
            Field(x => x.UnitAddressId, type: typeof(IdGraphType)).Description("Internal or external unit address id");
            Field(x => x.Remark, type: typeof(StringGraphType)).Description("Additional address information remark");
        }
    }

    public class AddressInfoInputType : InputObjectGraphType<AddressInfo>
    {
        public AddressInfoInputType()
        {
            Field(x => x.AccessAddressId, type: typeof(IdGraphType)).Description("Internal or external access address id");
            Field(x => x.UnitAddressId, type: typeof(IdGraphType)).Description("Internal or external unit address id");
            Field(x => x.Remark, type: typeof(StringGraphType)).Description("Additional address information remark");
        }
    }
}
