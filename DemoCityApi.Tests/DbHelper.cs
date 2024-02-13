using DemoCityApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoCityApi.Tests
{
    public static class DbHelper
    {
        public static CitiesContext GetTestDbContext(string dbName)
        {
            // Create db context options specifying in memory database
            var options = new DbContextOptionsBuilder<CitiesContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

            //Use this to instantiate the db context
            return new CitiesContext(options);
        }
    }
}
