using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;

using FluentAssertions;
using ApiUnitTesting.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiIntegrationTest.IntegrationTest
{
    public class EmployeeScenarios : EmployeeScenarioBase
    {
        [Fact]
        public async Task WhenGetAllEmployees_ReturnResponseStatusCodeOk()
        {
            // Arrange
            using EmployeeTestServer testServer = CreateEmployeeTestServer();
            using HttpClient httpClient = testServer.CreateClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(Get.GetAllEmployeesEndpoint);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenPostNewEmp_WhenGetEmpById_ReturnThePostedEmp()
        {
            // Arrange
            // Arrange
            using EmployeeTestServer testServer = CreateEmployeeTestServer();
            using HttpClient httpClient = testServer.CreateClient();

            Employee postEmployee = new()
            {
                Email = "test@gmail.com",
                DateOfBirth = DateTime.Now,
                FirstName = "test",
                LastName = "last",
                PhoneNumber = "001100223"
            };

            //await testServer.EmployeeContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [ApiTestDb].[dbo].[Employees] ON;"); NOT WORK SO WE RUN THIS IN SSMS
            await testServer.EmployeeContext.AddAsync(postEmployee);
            //this will persist the data in the real db
            await testServer.EmployeeContext.SaveChangesAsync();
            //await testServer.EmployeeContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [ApiTestDb].[dbo].[Employees] OFF;");

            // Act
            HttpResponseMessage response = await httpClient
                .GetAsync(Get.GetEmployeeByIdEndpoint + $"/{postEmployee.EmployeeId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Employee? content = await GetResponseContent<Employee>(response);

            content.Should().NotBeNull();
            content.Should().BeEquivalentTo(postEmployee);

            //not a good practice, should use in-memory db, and in teardown, we remove all record
            testServer.EmployeeContext.Remove(postEmployee);
            await testServer.EmployeeContext.SaveChangesAsync();
        }

    }
}
