using DemoCityApi.Business.Interfaces;
using DemoCityApi.Business.Models;
using DemoCityApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCityApi.Tests.Controllers
{
    [TestFixture]
    public class CitiesControllerTests
    {
        Mock<ICityManager> _mockCityManager;
        CitiesController _citiesController;
        [SetUp]
        public void Setup()
        {
            _mockCityManager = new Mock<ICityManager>();
            _citiesController = new CitiesController(_mockCityManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockCityManager.Reset();
        }

        [Test]
        public async Task SearchReturnsEmptyListWhenCityNameIsNull()
        {
            var searchResult = await _citiesController.Search("");
            Assert.IsInstanceOf<BadRequestResult>(searchResult);
        }

        [Test]
        public async Task SearchReturnsSearchResultsWhenCityExistingWithGivenName()
        {
            IList<CityInfoDto> returnCities = new List<CityInfoDto>() {
                    new CityInfoDto(){Name = "Hyderabad"}
                };
            _mockCityManager
                .Setup(mgr => mgr.SearchByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(returnCities);
            var searchResult = await _citiesController.Search("Hyderabad");
            Assert.IsInstanceOf<OkObjectResult>(searchResult);
            var returnValue = (searchResult as OkObjectResult).Value;
            Assert.IsInstanceOf<List<CityInfoDto>>(returnValue);
            Assert.True((returnValue as List<CityInfoDto>).Count == 1);
        }

        // TODO: Improve test coverage for the remaning actions
    }
}
