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
using ApiIntegrationTest.IntegrationTest.Data;

namespace ApiIntegrationTest.IntegrationTest
{
    public class EmployeeScenarios : EmployeeScenarioBase
    {
        public EmployeeScenarios(FixtureTestContainer fixtureTestContainer): base(fixtureTestContainer)
        {

        }

        [Fact]
        public async Task WhenGetAllEmployees_ReturnResponseStatusCodeOk()
        {
            // Arrange
            using EmployeeTestServer testServer = CreateEmployeeTestServer(base.FixtureTestContainer.ConnectionString);
            using HttpClient httpClient = testServer.CreateClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(Get.GetAllEmployeesEndpoint);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task WhenGetAllEmployees_ReturnValidResponse()
        {
            // Arrange
            using EmployeeTestServer testServer = CreateEmployeeTestServer(base.FixtureTestContainer.ConnectionString);
            using HttpClient httpClient = testServer.CreateClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(Get.GetAllEmployeesEndpoint);
            IEnumerable<Employee> allExpectedEmployees = EmployeeData.GetSampleEmployees();
            var actualEmployees = await GetResponseContent<IEnumerable<Employee>>(response);

            // Assert
            actualEmployees.Should().BeEquivalentTo(allExpectedEmployees);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(5)]
        //all sample employees id
        public async Task WhenGetEmployeeByExistingId_ReturnValidResponse(int empId)
        {
            // Arrange
            using EmployeeTestServer testServer = CreateEmployeeTestServer(base.FixtureTestContainer.ConnectionString);
            using HttpClient httpClient = testServer.CreateClient();

            // Act
            HttpResponseMessage response = await httpClient.GetAsync(Get.GetEmployeeByIdEndpoint + $"/{empId}");
            IEnumerable<Employee> allEmployees = EmployeeData.GetSampleEmployees();
            Employee expectedEmployee = allEmployees.FirstOrDefault(e => e.EmployeeId == empId);
            var actualEmployee = await GetResponseContent<Employee>(response);

            // Assert
            actualEmployee.Should().NotBeNull();
            expectedEmployee.Should().BeEquivalentTo(actualEmployee);
        }

        [Fact]
        public async Task GivenPostNewEmp_WhenGetEmpById_ReturnThePostedEmp()
        {
            // Arrange
            // Arrange
            using EmployeeTestServer testServer = CreateEmployeeTestServer(base.FixtureTestContainer.ConnectionString);
            using HttpClient httpClient = testServer.CreateClient();

            Employee postEmployee = new()
            {
                Email = "test@gmail.com",
                DateOfBirth = DateTime.Now,
                FirstName = "test",
                LastName = "last",
                PhoneNumber = "001100223"
            };

            await testServer.EmployeeContext.AddAsync(postEmployee);
            await testServer.EmployeeContext.SaveChangesAsync();

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
