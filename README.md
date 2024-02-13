# DemoCityApi
Sample web api application using Asp.Net core with unit testing and integration testing.

DemoCityApi - API layer. Contains the controllers and registering the dependencies.

DemoCityApi.Business - Business layer. Responsible for communicating with the other components such as database or any other third party services.

DemoCityApi.Data - Data layer. EntityFramework(with code first approach) is used to communicate with the database.

DemoCityApi.FunctionalTests - Contains the integration tests. Doesn't cover many scenarios. But it has the setup ready to extent further. In memory database is used for integration testing.

DemoCityApi.RestServices - Communicating with the third party web services is extacted into this separate project. 

DemoCityApi.Tests - It's unit test project. NUnit is used for unit testing. For unit testsing the business layer, I have mocked the DbContext. 


