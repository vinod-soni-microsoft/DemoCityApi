using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoCityApi.Data;
using DemoCityApi.Business.Interfaces;
using DemoCityApi.Business.Models;

namespace DemoCityApi.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityManager _cityManager;
        public CitiesController(ICityManager cityManager)
        {
            _cityManager = cityManager;
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id)
        {
            var city = await _cityManager.GetById(id);

            if (city == null)
            {
                return NotFound();
            }
            // CityDto model from Business layer is used here.
            // In production environment, I would not use a model from Business layer 
            // instead I will have the models created on the API layer and use those models for request and response payloads.
            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, CityDto city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            await _cityManager.UpdateAsync(city);
            return Ok();
        }

        // POST: api/Cities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(CityDto city)
        {
            var cityId = await _cityManager.CreateAsync(city);
            if (cityId < 0)
                return BadRequest();
            city.Id = cityId;
            return CreatedAtAction("GetCity", new { id = cityId }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            await _cityManager.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("search/{cityName}")]
        public async Task<IActionResult> Search(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return BadRequest();
            var searchResults = await _cityManager.SearchByNameAsync(cityName);
            return Ok(searchResults);
        }

    }
}
