using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ApiIntegrationTest.IntegrationTest.Extension;
using ApiUnitTesting.Api.Data;
using System.Text.Json;
using ApiIntegrationTest.IntegrationTest.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiIntegrationTest.IntegrationTest
{
    public class EmployeeScenarioBase
    {
        /// <summary>
        /// Create a test server represent our test application api
        /// It received a host builder to build host for managing application startup (we need to implement startup class and its methods)
        /// Everytimes this method call, it migrate db (if not exists) then ResetDb (Clear then Seed data)
        /// </summary>
        /// <returns></returns>
        public static EmployeeTestServer CreateEmployeeTestServer()
        {
            IWebHostBuilder hostBuilder = new WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((ctx, cfgBuilder) =>
            {
                cfgBuilder.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false)
                          .AddEnvironmentVariables();
            })
            .UseStartup<Startup>();

            EmployeeTestServer employeeTestServer = new(hostBuilder);
            //context is provide inside MigrateDatabase extension method
            employeeTestServer.Host.MigrateDatabase<AppDbContext>((context) => EmployeeContextSeeder.ResetDb(context));
            return employeeTestServer;
        }

        public static async Task<T?> GetResponseContent<T>(
        HttpResponseMessage httpResponseMessage)
        {
            JsonSerializerOptions jsonSettings = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            };

            return JsonSerializer.Deserialize<T>(
                await httpResponseMessage.Content.ReadAsStringAsync(),
                jsonSettings);
        }

        public static StringContent BuildRequestContent<T>(T content)
        {
            string serialized = JsonSerializer.Serialize(content);

            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        public const string EmployeeApiBaseUrl = "api/Employee/";

        public static class Get
        {
            public const string GetEmployeeByIdEndpoint = EmployeeApiBaseUrl + "GetEmployeeById";
            public const string GetAllEmployeesEndpoint = EmployeeApiBaseUrl + "GetAllEmployee";

        }

        public static class Post
        {
            public const string CreateEmployeeEndpoint = EmployeeApiBaseUrl + "CreateEmployee";
            public const string UpdateEmployeeEndpoint = EmployeeApiBaseUrl + "Employee";
        }


    }
}
