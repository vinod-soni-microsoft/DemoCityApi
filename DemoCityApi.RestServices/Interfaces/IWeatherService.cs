using DemoCityApi.RestServices.Models;
using System.Threading.Tasks;

namespace DemoCityApi.RestServices.Interfaces
{
    public interface IWeatherService
    {
        /// <summary>
        /// Gets 
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task<CurrentWeather> GetWeatherAsync(string city);
    }
}
