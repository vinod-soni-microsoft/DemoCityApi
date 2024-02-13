using DemoCityApi.RestServices.Interfaces;
using DemoCityApi.RestServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoCityApi.RestServices
{
    public class CountryInfoService : ICountryInfoService
    {
        private IHttpClientProvider _httpClientProvider;

        public CountryInfoService(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        public async Task<IList<CountryInfo>> GetCountryInfoAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return null;

            var client = _httpClientProvider.GetClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // https://restcountries.eu/rest/v2/name/india?fullText=true
            // TODO: Url should be read from the app settings
            var weatherInfo = await client.GetAsync("https://restcountries.eu/rest/v2/name/"+countryName+ "?fullText=true");
            if (weatherInfo.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IList<CountryInfo>>(await weatherInfo.Content.ReadAsStringAsync());
            }

            // TODO: Add logging if not received success code
            return null;
        }
    }
}
