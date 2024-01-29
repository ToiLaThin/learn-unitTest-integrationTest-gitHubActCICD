using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace ApiIntegrationTest.IntegrationTest
{
    public class FixtureTestContainer: IAsyncLifetime
    {
        private MsSqlContainer msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .Build();

        public string ConnectionString { get
            {
                return msSqlContainer.GetConnectionString();
            } 
        }

        public Task DisposeAsync()
        {
            return msSqlContainer.DisposeAsync().AsTask();
        }

        public Task InitializeAsync()
        {
            return msSqlContainer.StartAsync();
        }
    }
}
