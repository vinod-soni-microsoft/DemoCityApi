using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoCityApi.Models
{
    public class CityInfo
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Rating { get; set; }
        public DateTime EstablishedDate { get; set; }
        public int EstimatedPopulation { get; set; }
        public CountryInfo CountryInfo { get; set; }
        public WeatherInfo Weather { get; set; }
    }
}
