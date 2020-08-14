using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.APIGateway.Remote
{
    public class QueryServiceClient<QueryServiceType>
    {
        private readonly ILogger<QueryServiceClient<QueryServiceType>> _logger;
        private readonly string _serviceUrl;

        public QueryServiceClient(ILogger<QueryServiceClient<QueryServiceType>> logger, string serviceUrl)
        {
            _logger = logger;
            _serviceUrl = serviceUrl;
        }

        public async Task<ResponseType> Query<RequestType, ResponseType>(RequestType queryCommand)
        {
            using (var httpClient = new HttpClient())
            {
                string requestUrl = _serviceUrl + "/query";

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestUrl),
                    Content = new StringContent(JsonConvert.SerializeObject(queryCommand), Encoding.UTF8, "application/json")
                };

                _logger.LogDebug($"Sending query request: {queryCommand.GetType().Name} to service: {requestUrl}");

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return (ResponseType)JsonConvert.DeserializeObject(responseBody, typeof(ResponseType));
            }
        }
    }
}
