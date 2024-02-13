using DemoCityApi.RestServices.Interfaces;
using System.Net.Http;

namespace DemoCityApi.RestServices
{
    public class HttpClientProvider : IHttpClientProvider
    {
        HttpMessageHandler _messageHandler = null;
        public HttpClientProvider(HttpMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public HttpClient GetClient()
        {
            // TODO: Implement singleton to make sure HttpClient is not created for every call 
            return new HttpClient(_messageHandler);
        }
    }
}
