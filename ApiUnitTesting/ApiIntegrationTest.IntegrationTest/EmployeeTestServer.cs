using ApiUnitTesting.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegrationTest.IntegrationTest
{
    /// <summary>
    /// This test service have EmployeeContext property to access real database
    /// </summary>
    public class EmployeeTestServer : TestServer
    {
        public AppDbContext EmployeeContext { get; set; }
        /// <summary>
        /// This method call its base contructor TestServer(IWebHostBuilder hostBuilder), 
        /// then init the EmployeeContext property
        /// </summary>
        /// <param name="builder"></param>
        public EmployeeTestServer(IWebHostBuilder builder): base(builder) {
            EmployeeContext = Host.Services.GetRequiredService<AppDbContext>();
        }
    }
}
