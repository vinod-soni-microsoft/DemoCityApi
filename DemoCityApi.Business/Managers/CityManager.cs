using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCityApi.Business.Extensions;
using DemoCityApi.Business.Interfaces;
using DemoCityApi.Business.Models;
using DemoCityApi.Data;
using DemoCityApi.RestServices.Interfaces;


namespace DemoCityApi.Business.Managers
{
    public class CityManager : ICityManager
    {
        private readonly CitiesContext _citiesContext;
        private readonly IWeatherService _weatherService;
        private readonly ICountryInfoService _countryInfoService;
        public CityManager(CitiesContext citiesContext, 
            IWeatherService weatherService,
            ICountryInfoService countryInfoService)
        {
            _citiesContext = citiesContext;
            _weatherService = weatherService;
            _countryInfoService = countryInfoService;
        }

        public async Task<CityDto> GetById(int cityId)
        {
            if (cityId <= 0)
            {
                return null;
            }
            var city = await _citiesContext.Cities.FindAsync(cityId);
            if (city == null)
                return null;
            return city.ToDto();
        }

        public async Task<int> CreateAsync(CityDto cityDto)
        {
            if (cityDto != null &&
                string.IsNullOrWhiteSpace(cityDto.Name) == false)
            {
                // depending on the business rules, vaidate the DTO.
                var city = cityDto.ToCity();
                _citiesContext.Cities.Add(city);
                await _citiesContext.SaveChangesAsync();
                return city.Id;
            }
            return -1;
        }

        public async Task DeleteAsync(int cityId)
        {
            if (cityId > 0)
            {
                var city = await _citiesContext.Cities.FindAsync(cityId);
                if (city != null)
                {
                    _citiesContext.Cities.Remove(city);
                    await _citiesContext.SaveChangesAsync();
                }
            }
        }

        public async Task<IList<CityInfoDto>> SearchByNameAsync(string cityName)
        {
            var citiesToReturn = new List<CityInfoDto>();

            if (string.IsNullOrWhiteSpace(cityName))
                return citiesToReturn;

            var cities = await _citiesContext.Cities.Where(city => city.Name != null &&
            city.Name.Contains(cityName)).ToListAsync();

            if (cities.Count == 0)
                return citiesToReturn;

            foreach (var city in cities)
            {
                var cityDto = city.ToInfoDto();
                await UpdateWeatherAsync(cityDto);
                await UpdateContryInfoAsync(cityDto);
                citiesToReturn.Add(cityDto);
            }

            return citiesToReturn;
        }

        private async Task UpdateContryInfoAsync(CityInfoDto cityDto)
        {
            if (string.IsNullOrWhiteSpace(cityDto.Country))
            {
                return;
            }
            var countryInfo = await _countryInfoService.GetCountryInfoAsync(cityDto.Country);
            if (countryInfo == null || countryInfo.Count == 0)
                return;
            cityDto.CountryInfo = new CountryInfoDto()
            {
                Alpha2Code = countryInfo[0].Alpha2Code,
                Alpha3Code = countryInfo[0].Alpha3Code,
                Currencies = countryInfo[0].Currencies?.Select(cu => cu.Name).ToList()
            };
        }

        private async Task UpdateWeatherAsync(CityInfoDto cityDto)
        {
            var weatherInfo = await _weatherService.GetWeatherAsync(cityDto.Name);
            if(weatherInfo != null)
            {
                cityDto.Weather = new WeatherInfoDto()
                {
                    Description = weatherInfo.Weather?.ElementAtOrDefault(0)?.Description,
                    Humidity = weatherInfo.Main?.Humidity,
                    Pressure = weatherInfo.Main?.Pressure,
                    Temperature = new TemperatureDto()
                    {
                        Current = weatherInfo.Main?.Temp,
                        FeelsLike = weatherInfo.Main?.FeelsLike,
                        Minimum = weatherInfo.Main?.TempMin,
                        Maximum = weatherInfo.Main?.TempMax,
                    }
                };
            }
        }

        public async Task UpdateAsync(CityDto cityDto)
        {
            if (cityDto == null)
            {
                return;
            }
            var cityToUpdate = cityDto.ToCity();
            var existingCity = await _citiesContext.Cities.FindAsync(cityDto.Id);
            if (existingCity != null)
            {
                _citiesContext.Update(cityToUpdate);
                await _citiesContext.SaveChangesAsync();
            }
        }
    }
}
