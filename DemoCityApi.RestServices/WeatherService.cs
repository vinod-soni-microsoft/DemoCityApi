using DemoCityApi.RestServices.Interfaces;
using DemoCityApi.RestServices.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DemoCityApi.RestServices
{
    public class WeatherService : IWeatherService
    {
        private IHttpClientProvider _httpClientProvider;
        
        public WeatherService(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public async Task<CurrentWeather> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return null;

            var client = _httpClientProvider.GetClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           
            // TODO: Url should be read from the app settings
            var weatherInfo = await client.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=london&appid=04c73b7a673ebef725d3765edff2c31d&units=metric");
            if (weatherInfo.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<CurrentWeather>(await weatherInfo.Content.ReadAsStringAsync());
            }

            // TODO: Add logging if not received success code
            return null;
        }
    }
}
