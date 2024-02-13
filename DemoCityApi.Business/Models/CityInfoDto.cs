namespace DemoCityApi.Business.Models
{
    public class CityInfoDto : CityDto
    {   
        public CountryInfoDto CountryInfo { get; set; }
        public WeatherInfoDto Weather { get; set; }
    }
}
