namespace DemoCityApi.Models
{
    public class WeatherInfo
    {
        public string Description { get; set; }
        public Temperature Temperature { get; set; }
        public decimal Pressure { get; set; }
        public decimal Humidity { get; set; }
    }
}
