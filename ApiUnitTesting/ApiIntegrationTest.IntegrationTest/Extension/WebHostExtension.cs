using ApiIntegrationTest.IntegrationTest.Data;
using ApiUnitTesting.Api.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegrationTest.IntegrationTest.Extension
{
    internal static class WebHostExtension
    {
        /// <summary>
        /// access host to get the DbContext(generic TDbContext) for migration
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="webHost"></param>
        public static void MigrateDatabase<TContext>(
            this IWebHost webHost,
            Action<TContext> seeder
        ) where TContext: notnull, DbContext
        {
            using (var serviceScope = webHost.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;
                TContext context = services.GetService<TContext>();

                try {
                    context.Database.Migrate();
                    seeder(context);                    
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }
    }

    
}
