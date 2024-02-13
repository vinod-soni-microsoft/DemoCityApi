using DemoCityApi.RestServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoCityApi.RestServices.Interfaces
{
    public interface ICountryInfoService
    {
        Task<IList<CountryInfo>> GetCountryInfoAsync(string countryName);
    }
}
