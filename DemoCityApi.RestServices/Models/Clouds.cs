using Newtonsoft.Json;

namespace DemoCityApi.RestServices.Models
{
    public class Clouds
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }
}
