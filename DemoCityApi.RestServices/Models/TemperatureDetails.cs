using Newtonsoft.Json;

namespace DemoCityApi.RestServices.Models
{
    public class TemperatureDetails
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public decimal Pressure { get; set; }

        [JsonProperty("humidity")]
        public decimal Humidity { get; set; }
    }
}
