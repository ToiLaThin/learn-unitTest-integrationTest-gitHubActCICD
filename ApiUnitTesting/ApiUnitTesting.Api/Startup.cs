using ApiUnitTesting.Api.Data;
using ApiUnitTesting.Api.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegrationTest.IntegrationTest
{
    /// <summary>
    /// Copy of startup in Program.cs of ApiUnitTesting.Api
    /// could implement IStartup interface
    /// must be place in Api project (not the IntegrationTest project) otherwise MapController does not work(route is not right)
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Config Services Collection before startup, name must match ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            string connString = Configuration.GetConnectionString("DefaultConnString");
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connString);
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        /// <summary> 
        /// Config middlewares before startup, name must match Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
