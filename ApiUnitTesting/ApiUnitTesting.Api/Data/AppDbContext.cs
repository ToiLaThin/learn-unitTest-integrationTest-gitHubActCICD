using ApiUnitTesting.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiUnitTesting.Api.Data
{
    public class AppDbContext: DbContext
    {
        DbSet<Employee> Employees { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
