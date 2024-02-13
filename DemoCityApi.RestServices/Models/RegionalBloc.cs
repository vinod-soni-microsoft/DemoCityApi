using Newtonsoft.Json;

namespace DemoCityApi.RestServices.Models
{
    public class RegionalBloc
    {
        [JsonProperty("acronym")]
        public string Acronym { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("otherAcronyms")]
        public object[] OtherAcronyms { get; set; }

        [JsonProperty("otherNames")]
        public string[] OtherNames { get; set; }
    }
}
