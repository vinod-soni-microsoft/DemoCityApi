using System.Net.Http;

namespace DemoCityApi.RestServices.Interfaces
{
    public interface IHttpClientProvider
    {
        HttpClient GetClient();
    }
}
