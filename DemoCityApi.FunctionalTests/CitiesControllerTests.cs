using DemoCityApi.Business.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoCityApi.FunctionalTests
{
    [TestFixture]
    public class CitiesControllerTests
    {
        private HttpClient _client;

        public CitiesControllerTests()
        {
            _client = (new ApiWebApplicationFactory()).CreateClient();
        }


        [Test]
        public async Task GetEndpointReturnsCityWhenCityIdIsExistingInDb()
        {
            // Act
            var response = await _client.GetAsync("api/cities/1");
            response.EnsureSuccessStatusCode();
            var responseCity = JsonConvert.DeserializeObject<CityDto>( await response.Content.ReadAsStringAsync());
            // Assert
            Assert.AreEqual(1, responseCity.Id);
            Assert.AreEqual("Cambridge", responseCity.Name);
            Assert.AreEqual("UK", responseCity.Country);
        }

        [Test]
        public async Task GetEndpointReturnsNotFoundResultWhenCityIdIsNotExistingInDb()
        {
            // Act
            var response = await _client.GetAsync("api/cities/999");
            Assert.AreEqual(404, (int)response.StatusCode);
        }
    }
}
