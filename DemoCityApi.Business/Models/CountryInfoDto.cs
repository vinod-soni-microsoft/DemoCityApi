using System.Collections.Generic;

namespace DemoCityApi.Business.Models
{
    public class CountryInfoDto
    {
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public IList<string> Currencies { get; set; }
    }
}
