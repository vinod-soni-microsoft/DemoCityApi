using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace DemoCityApi.Data
{
    public class CitiesContext : DbContext
    {

        public CitiesContext(DbContextOptions<CitiesContext> options) : base(options)
        {
        }

        public CitiesContext()
        {
            // For mocking
        }

        public virtual DbSet<City> Cities { get; set; }
    }
}
