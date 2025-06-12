using OpenFTTH.Results;

namespace OpenFTTH.RouteNetwork.API.Queries
{
    public class FindNearestRouteSegmentsError : Error
    {
        public FindNearestRouteSegmentsErrorCodes Code { get; }

        public FindNearestRouteSegmentsError(FindNearestRouteSegmentsErrorCodes errorCode, string errorMsg) : base(errorCode.ToString() + ": " + errorMsg)
        {
            this.Code = errorCode;
            Metadata.Add("ErrorCode", errorCode.ToString());
        }
    }
}
