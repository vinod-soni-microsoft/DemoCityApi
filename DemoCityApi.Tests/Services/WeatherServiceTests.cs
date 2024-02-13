using DemoCityApi.RestServices;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;
using DemoCityApi.RestServices.Interfaces;
using System.Net.Http;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace DemoCityApi.Tests.Services
{
    [TestFixture]
    public class WeatherServiceTests
    {
        IHttpClientProvider _httpClientProvider;
        Mock<HttpMessageHandler> _mockHttpMessageHandler;
        WeatherService _weatherService;

        #region Setup and TearDown
        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClientProvider = new HttpClientProvider(_mockHttpMessageHandler.Object);
            _weatherService = new WeatherService(_httpClientProvider);
        }

        [TearDown]
        public void TearDown()
        {
            _mockHttpMessageHandler.Reset();
            _httpClientProvider = null;
        }
        #endregion

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public async Task GetWeatherAsyncReturnsNullWhenInputCityIsInvalid(string city)
        {
            var currentWeather = await _weatherService.GetWeatherAsync(city);
            Assert.Null(currentWeather);
        }

        [Test]
        public async Task GetWeatherAsyncReturnsNullWhenSuccessResponseNotReceived()
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("not found")
                });
            var currentWeather = await _weatherService.GetWeatherAsync("london");
            Assert.Null(currentWeather);
        }

        [Test]
        public async Task GetWeatherAsyncReturnsCurrentWeatherWhenReceivedResponseSuccessfully()
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"coord\":{\"lon\":-0.13,\"lat\":51.51},\"weather\":[{\"id\":803,\"main\":\"Clouds\",\"description\":\"broken clouds\",\"icon\":\"04n\"}],\"base\":\"stations\",\"main\":{\"temp\":8.45,\"feels_like\":4.95,\"temp_min\":7.78,\"temp_max\":9.44,\"pressure\":1014,\"humidity\":86},\"visibility\":10000,\"wind\":{\"speed\":3.76,\"deg\":230},\"clouds\":{\"all\":52},\"dt\":1608481230,\"sys\":{\"type\":3,\"id\":2019646,\"country\":\"GB\",\"sunrise\":1608451412,\"sunset\":1608479581},\"timezone\":0,\"id\":2643743,\"name\":\"London\",\"cod\":200}")
                });
            var currentWeather = await _weatherService.GetWeatherAsync("london");
            Assert.NotNull(currentWeather);
        }

        // TODO: Add tests to validate the data
    }
}
