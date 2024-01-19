using ApiUnitTesting.Api.Model;
using ApiUnitTesting.Api.Repo;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTesting.UnitTest.Repo.Mock.Contract
{
    internal interface IRepositoryWrapper
    {
        public Mock<IGenericRepository<Employee>> MockEmployeeRepo { get; set; }
    }
}
