using DemoCityApi.Business;
using DemoCityApi.Business.Extensions;
using DemoCityApi.Business.Managers;
using DemoCityApi.Business.Models;
using DemoCityApi.Data;
using DemoCityApi.RestServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoCityApi.Tests.Business
{
    [TestFixture]
    public class CityManagerTests
    {
        CityManager _cityManager;
        Mock<CitiesContext> _mockCityContext;
        Mock<DbSet<City>> _mockCityDbSet;
        Mock<IWeatherService> _mockWeatherService;
        Mock<ICountryInfoService> _mockCountryInfoService;

        #region Setup TearDown

        [SetUp]
        public void Setup()
        {
            var citiesToReturn = new List<City>() { 
                new City()
                {
                    Id = 1,
                    Country = "India",
                    EstablishedDate = DateTime.UtcNow.AddYears(-20),
                    EstimatedPopulation = 123456789,
                    Name = "Hyderabad",
                    Rating = 5,
                    State = "TG"
                },
                new City()
                {
                    Id = 2,
                    Country = "UK",
                    EstablishedDate = DateTime.UtcNow.AddYears(-22),
                    EstimatedPopulation = 42345678,
                    Name = "London",
                    Rating = 5,
                    State = "London"
                },
                new City()
                {
                    Id = 3,
                    Country = "UK",
                    EstablishedDate = DateTime.UtcNow.AddYears(-10),
                    EstimatedPopulation = 22345678,
                    Name = "Cambridge",
                    Rating = 5,
                    State = "Cambridgeshire"
                }
            };
            _mockCityContext = new Mock<CitiesContext>();
            _mockCityDbSet = new Mock<DbSet<City>>();
            _mockWeatherService = new Mock<IWeatherService>();
            _mockCountryInfoService = new Mock<ICountryInfoService>();

            var queryable = citiesToReturn.AsQueryable();
            _mockCityDbSet.As<IQueryable<City>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _mockCityDbSet.As<IQueryable<City>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _mockCityDbSet.As<IQueryable<City>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _mockCityDbSet.As<IQueryable<City>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            _mockCityContext.Setup(s => s.Cities).Returns(_mockCityDbSet.Object);
            _cityManager = new CityManager(_mockCityContext.Object, 
                _mockWeatherService.Object,
                _mockCountryInfoService.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _mockCityDbSet.Reset();
            _mockCityContext.Reset();
        }

        #endregion

        #region Tests

        [Test]
        public async Task CreateShouldNotThrowExceptionWhenCityDtoIsNull()
        {
            await _cityManager.CreateAsync(null);
        }

        [Test]
        public async Task CreateShouldCallSaveChangesWhenProvidedValidInputCity()
        {
            await _cityManager.CreateAsync(GetTestCityDto());
            _mockCityDbSet.Verify(dbSet => dbSet.Add(It.IsAny<City>()), Times.Once);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CreateShouldNotCallSaveChangesWhenCityNameIsNull()
        {
            await _cityManager.CreateAsync(new CityDto() { Name = null });
            _mockCityDbSet.Verify(dbSet => dbSet.Add(It.IsAny<City>()), Times.Never);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DeleteRemovesCityWhenCityIdIsValid()
        {
            int testCityId = 123;
            City mockCity = new City()
            {
                Id = testCityId
            };
            _mockCityDbSet.Setup(dbset => dbset.FindAsync(testCityId)).ReturnsAsync(mockCity);
            await _cityManager.DeleteAsync(testCityId);
            _mockCityDbSet.Verify(dbSet => dbSet.Remove(mockCity), Times.Once);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteRemovesCityNotCalledWhenCityNotExistsWithGivenId()
        {
            int testCityId = 123;
            City mockCity = null;
            _mockCityDbSet.Setup(dbset => dbset.FindAsync(testCityId)).ReturnsAsync(mockCity);
            await _cityManager.DeleteAsync(testCityId);
            _mockCityDbSet.Verify(dbSet => dbSet.Remove(It.IsAny<City>()), Times.Never);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task DeleteShouldNotCallFindWhenCityIdIsInvalid()
        {
            int testCityId = -1;
            City mockCity = null;
            _mockCityDbSet.Setup(dbset => dbset.FindAsync(testCityId)).ReturnsAsync(mockCity);
            await _cityManager.DeleteAsync(testCityId);
            _mockCityDbSet.Verify(dbSet => dbSet.FindAsync(It.IsAny<int>()), Times.Never);
            _mockCityDbSet.Verify(dbSet => dbSet.Remove(It.IsAny<City>()), Times.Never);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateShouldNotCallFindWhenCityIsNull()
        {
            await _cityManager.UpdateAsync(null);
        }

        [Test]
        public async Task UpdateShouldCallSaveChangesWhenInputCityIsValid()
        {
            int testCityId = 123;
            var mockCity = GetTestCityDto(testCityId);
            _mockCityDbSet.Setup(dbset => dbset.FindAsync(testCityId)).ReturnsAsync(mockCity.ToCity());
            await _cityManager.UpdateAsync(mockCity);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateShouldNotCallSaveChangesWhenCityNotExistsWithGivenId()
        {
            int testCityId = 123;
            var mockCity = GetTestCityDto(testCityId);
            City returnCity = null;
            _mockCityDbSet.Setup(dbset => dbset.FindAsync(testCityId)).ReturnsAsync(returnCity);
            await _cityManager.UpdateAsync(mockCity);
            _mockCityContext.Verify(ct => ct.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public async Task SearchReturnsEmptyListWhenCityNameIsNullOrEmpty(string cityName)
        {
            var cities = await _cityManager.SearchByNameAsync(cityName);
            Assert.NotNull(cities);
            Assert.True(cities.Count == 0);
        }
        
        [Ignore("IAsyncProvider")]
        [Test]
        public async Task SearchShouldCallWeatheServiceWhenSearchResultExists()
        {
            string cityName = "Hyderabad";
            var cities = await _cityManager.SearchByNameAsync(cityName);
            _mockWeatherService.Verify(s => s.GetWeatherAsync(cityName), Times.Once);   
        }

        #endregion

        #region Private methods

        private static CityDto GetTestCityDto(int cityId = 0)
        {
            return new CityDto()
            {
                Name = "Test",
                Country = "Test Country",
                EstablishedDate = DateTime.UtcNow.AddYears(-2),
                EstimatedPopulation = 124567,
                Rating = 3,
                State = "Test state",
                Id = cityId
            };
        }

        #endregion
    }
}
