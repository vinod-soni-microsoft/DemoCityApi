using DemoCityApi.Business.Models;
using DemoCityApi.Data;

namespace DemoCityApi.Business.Extensions
{
    public static class CityDtoExtensions
    {
        public static City ToCity(this CityDto cityDto)
        {
            return new City() {
                Name = cityDto.Name,
                Country = cityDto.Country,
                EstablishedDate = cityDto.EstablishedDate,
                EstimatedPopulation = cityDto.EstimatedPopulation,
                Id = cityDto.Id,
                Rating = cityDto.Rating,
                State = cityDto.State
            };
        }

        public static CityDto ToDto(this City city)
        {
            return new CityDto()
            {
                Country = city.Country,
                EstablishedDate = city.EstablishedDate,
                EstimatedPopulation = city.EstimatedPopulation,
                Id = city.Id,
                Name = city.Name,
                Rating = city.Rating,
                State = city.State
            };
        }

        public static CityInfoDto ToInfoDto(this City city)
        {
            return new CityInfoDto()
            {
                Country = city.Country,
                EstablishedDate = city.EstablishedDate,
                EstimatedPopulation = city.EstimatedPopulation,
                Id = city.Id,
                Name = city.Name,
                Rating = city.Rating,
                State = city.State
            };
        }
    }
}
