namespace DemoCityApi.Business.Models
{
    public class WeatherInfoDto
    {
        public string Description { get; set; }
        public TemperatureDto Temperature { get; set; }
        public decimal? Pressure { get; set; }
        public decimal? Humidity { get; set; }
    }
}
