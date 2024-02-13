using DemoCityApi.RestServices;
using DemoCityApi.RestServices.Interfaces;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCityApi.Tests.Services
{
    [TestFixture]
    public class CountryServiceTests
    {
        IHttpClientProvider _httpClientProvider;
        Mock<HttpMessageHandler> _mockHttpMessageHandler;
        CountryInfoService _countryInfoService;

        #region Setup and TearDown
        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClientProvider = new HttpClientProvider(_mockHttpMessageHandler.Object);
            _countryInfoService = new CountryInfoService(_httpClientProvider);
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
        public async Task GetCountryInfoAsyncReturnsNullWhenInputCountryIsInvalid(string country)
        {
            var currentWeather = await _countryInfoService.GetCountryInfoAsync(country);
            Assert.Null(currentWeather);
        }

        [Test]
        public async Task GetCountryInfoAsyncReturnsNullWhenSuccessResponseNotReceived()
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("not found")
                });
            var currentWeather = await _countryInfoService.GetCountryInfoAsync("london");
            Assert.Null(currentWeather);
        }

        [Test]
        public async Task GetCountryInfoAsyncReturnsCountryInfoWhenReceivedResponseSuccessfully()
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{\"name\":\"British Indian Ocean Territory\",\"topLevelDomain\":[\".io\"],\"alpha2Code\":\"IO\",\"alpha3Code\":\"IOT\",\"callingCodes\":[\"246\"],\"capital\":\"Diego Garcia\",\"altSpellings\":[\"IO\"],\"region\":\"Africa\",\"subregion\":\"Eastern Africa\",\"population\":3000,\"latlng\":[-6.0,71.5],\"demonym\":\"Indian\",\"area\":60.0,\"gini\":null,\"timezones\":[\"UTC+06:00\"],\"borders\":[],\"nativeName\":\"British Indian Ocean Territory\",\"numericCode\":\"086\",\"currencies\":[{\"code\":\"USD\",\"name\":\"United States dollar\",\"symbol\":\"$\"}],\"languages\":[{\"iso639_1\":\"en\",\"iso639_2\":\"eng\",\"name\":\"English\",\"nativeName\":\"English\"}],\"translations\":{\"de\":\"Britisches Territorium im Indischen Ozean\",\"es\":\"Territorio Británico del Océano Índico\",\"fr\":\"Territoire britannique de l'océan Indien\",\"ja\":\"イギリス領インド洋地域\",\"it\":\"Territorio britannico dell'oceano indiano\",\"br\":\"Território Britânico do Oceano íÍdico\",\"pt\":\"Território Britânico do Oceano Índico\",\"nl\":\"Britse Gebieden in de Indische Oceaan\",\"hr\":\"Britanski Indijskooceanski teritorij\",\"fa\":\"قلمرو بریتانیا در اقیانوس هند\"},\"flag\":\"https://restcountries.eu/data/iot.svg\",\"regionalBlocs\":[{\"acronym\":\"AU\",\"name\":\"African Union\",\"otherAcronyms\":[],\"otherNames\":[\"الاتحاد الأفريقي\",\"Union africaine\",\"União Africana\",\"Unión Africana\",\"Umoja wa Afrika\"]}],\"cioc\":\"\"},{\"name\":\"India\",\"topLevelDomain\":[\".in\"],\"alpha2Code\":\"IN\",\"alpha3Code\":\"IND\",\"callingCodes\":[\"91\"],\"capital\":\"New Delhi\",\"altSpellings\":[\"IN\",\"Bhārat\",\"Republic of India\",\"Bharat Ganrajya\"],\"region\":\"Asia\",\"subregion\":\"Southern Asia\",\"population\":1295210000,\"latlng\":[20.0,77.0],\"demonym\":\"Indian\",\"area\":3287590.0,\"gini\":33.4,\"timezones\":[\"UTC+05:30\"],\"borders\":[\"AFG\",\"BGD\",\"BTN\",\"MMR\",\"CHN\",\"NPL\",\"PAK\",\"LKA\"],\"nativeName\":\"भारत\",\"numericCode\":\"356\",\"currencies\":[{\"code\":\"INR\",\"name\":\"Indian rupee\",\"symbol\":\"₹\"}],\"languages\":[{\"iso639_1\":\"hi\",\"iso639_2\":\"hin\",\"name\":\"Hindi\",\"nativeName\":\"हिन्दी\"},{\"iso639_1\":\"en\",\"iso639_2\":\"eng\",\"name\":\"English\",\"nativeName\":\"English\"}],\"translations\":{\"de\":\"Indien\",\"es\":\"India\",\"fr\":\"Inde\",\"ja\":\"インド\",\"it\":\"India\",\"br\":\"Índia\",\"pt\":\"Índia\",\"nl\":\"India\",\"hr\":\"Indija\",\"fa\":\"هند\"},\"flag\":\"https://restcountries.eu/data/ind.svg\",\"regionalBlocs\":[{\"acronym\":\"SAARC\",\"name\":\"South Asian Association for Regional Cooperation\",\"otherAcronyms\":[],\"otherNames\":[]}],\"cioc\":\"IND\"}]")
                });
            var countryInfo = await _countryInfoService.GetCountryInfoAsync("india");
            Assert.NotNull(countryInfo);
        }

        // TODO: Add tests to validate the data
    }
}
