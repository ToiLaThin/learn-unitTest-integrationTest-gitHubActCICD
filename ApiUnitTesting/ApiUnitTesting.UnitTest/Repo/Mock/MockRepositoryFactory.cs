using ApiUnitTesting.Api.Model;
using ApiUnitTesting.Api.Repo;
using ApiUnitTesting.UnitTest.Data;
using ApiUnitTesting.UnitTest.Repo.Mock.Contract;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUnitTesting.UnitTest.Repo.Mock
{
    internal class MockRepositoryFactory
    {
        public static Mock<IGenericRepository<Employee>> GetEmployeeRepoMock()
        {
            var mock = new Mock<IGenericRepository<Employee>>();

            List<Employee> employees = EmployeeData.GetSampleEmployees();
            mock.Setup(m => m.GetAll()).Returns(employees);
            mock.Setup(m => m.GetAsQueryable()).Returns(employees.AsQueryable());
            mock.Setup(m => m.GetById(It.IsAny<object>()))
                .Returns<object>((id) => employees.FirstOrDefault(e => e.EmployeeId.Equals(id)));
            mock.Setup(m => m.Insert(It.IsAny<Employee>())).Callback(() => { return; });
            mock.Setup(m => m.Delete(It.IsAny<Employee>())).Callback(() => { return; });
            mock.Setup(m => m.Update(It.IsAny<Employee>())).Callback(() => { return; });

            return mock;
        }
        public static Mock<IRepositoryWrapper> GetMockWrapper()
        {
            var mockWrapper = new Mock<IRepositoryWrapper>();
            //this have its methods set up not just empty new Mock<IGenericRepository<Employee>>()
            var employeeGenericRepoMock = GetEmployeeRepoMock();
            mockWrapper.Setup(mW => mW.MockEmployeeRepo).Returns(() => employeeGenericRepoMock);
            return mockWrapper;
        }
    }
}
