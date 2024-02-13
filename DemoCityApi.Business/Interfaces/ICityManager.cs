using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCityApi.Business.Models;


namespace DemoCityApi.Business.Interfaces
{
    public interface ICityManager
    {
        Task<CityDto> GetById(int cityId);
        Task<int> CreateAsync(CityDto cityDto);
        Task UpdateAsync(CityDto cityDto);
        Task<IList<CityInfoDto>> SearchByNameAsync(string city);
        Task DeleteAsync(int cityId);
    }
}
