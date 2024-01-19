using ApiUnitTesting.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegrationTest.IntegrationTest.Data
{
    /// <summary>
    /// Seed a context data
    /// </summary>
    public class EmployeeContextSeeder
    {
        private static void TruncateDb(DbContext context)
        {
            var entities = new[]
            {
                typeof(Employee).FullName
            };
            foreach (var entityName in entities)
            {
                var entityType = context.Model.FindEntityType(entityName);
                var tableName = entityType.GetTableName();
                var schema = entityType.GetSchema();
                context.Database.ExecuteSqlRaw($"DELETE FROM {schema}.{tableName}");
            }
        }
        /// <summary>
        /// TODO make this method more generic
        /// </summary>
        /// <param name="context"></param>
        private static void SeedDb(DbContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            var metaData = context.Model.FindEntityType(typeof(Employee).FullName);
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON");
            context.AddRange(EmployeeData.GetSampleEmployees());
            context.SaveChanges();
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF");
            transaction.Commit();
        }
        /// <summary>
        /// Clear Db using Delete From SQL
        /// Then Insert Sample Data
        /// </summary>
        /// <param name="context"></param>
        public static void ResetDb(DbContext context)
        {
            TruncateDb(context);
            SeedDb(context);

        }
    }
}
