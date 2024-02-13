using Newtonsoft.Json;

namespace DemoCityApi.RestServices.Models
{
    public class Language
    {
        [JsonProperty("iso639_1")]
        public string Iso6391 { get; set; }

        [JsonProperty("iso639_2")]
        public string Iso6392 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nativeName")]
        public string NativeName { get; set; }
    }
}
