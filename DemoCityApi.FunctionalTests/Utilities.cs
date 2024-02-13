using System;
using System.Collections.Generic;
using DemoCityApi.Data;

namespace DemoCityApi.FunctionalTests
{
    public static class Utilities
    {   
        public static void InitializeDbForTests(CitiesContext db)
        {
            db.Cities.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(CitiesContext db)
        {
            db.Cities.RemoveRange(db.Cities);
            InitializeDbForTests(db);
        }

        public static List<City> GetSeedingMessages()
        {
            return new List<City>()
            {
                new City(){
                    Id=1, 
                    Name="Cambridge", 
                    Country="UK", 
                    EstablishedDate=DateTime.UtcNow.AddDays(-10)
                }
            };
        }
    }
}
