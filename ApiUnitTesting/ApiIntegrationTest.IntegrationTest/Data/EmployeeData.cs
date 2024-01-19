using ApiUnitTesting.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegrationTest.IntegrationTest.Data
{
    internal class EmployeeData
    {
        public static List<Employee> GetSampleEmployees()
        {
            List<Employee> output = new List<Employee>
            {
                new Employee
                {
                    FirstName = "Jhon",
                    LastName = "Doe",
                    PhoneNumber = "01682616789",
                    DateOfBirth = new DateTime(2024,1,1),
                    Email = "jhon@gmail.com",
                    EmployeeId = 1
                },
                new Employee
                {
                    FirstName = "Jhon1",
                    LastName = "Doe1",
                    PhoneNumber = "01682616787",
                    DateOfBirth = new DateTime(2024,1,1),
                    Email = "jhon@gmail.com",
                    EmployeeId = 4
                },
                new Employee
                {
                    FirstName = "Jhon2",
                    LastName = "Doe2",
                    PhoneNumber = "01682616787",
                    DateOfBirth = new DateTime(2024,1,1),
                    Email = "jhon2@gmail.com",
                    EmployeeId = 5
                }
            };
            return output;
        }
    }
}
