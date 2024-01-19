using ApiUnitTesting.Api.Controllers;
using ApiUnitTesting.Api.Model;
using ApiUnitTesting.Api.Repo;
using ApiUnitTesting.UnitTest.Data;
using ApiUnitTesting.UnitTest.Repo.Mock;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiUnitTesting.UnitTest.Controller
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IGenericRepository<Employee>> _mockRepo;
        public EmployeeControllerTests()
        {
            _mockRepo = MockRepositoryFactory.GetEmployeeRepoMock();
        }

        [Fact]
        //naming convention MethodName_expectedBehavior_StateUnderTest
        public void GetEmployee_ListOfEmployee_EmployeeExistsInRepo()
        {
            //arrange
            var controller = new EmployeeController(_mockRepo.Object);

            //act
            var actionResult = controller.GetEmployee();
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as IEnumerable<Employee>;

            //assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(EmployeeData.GetSampleEmployees().Count(), actual.Count());
        }

        [Fact]
        public void GetEmployeeById_EmployeeObject_EmployeewithSpecificeIdExists()
        {
            //arrange
            var employees = EmployeeData.GetSampleEmployees();
            var firstEmployee = employees[0];

            //re setup to avoid assert failed due to mismatch in date property
            _mockRepo.Setup(m => m.GetById(It.IsAny<object>())).Returns(firstEmployee);
            var controller = new EmployeeController(_mockRepo.Object);

            //act
            var actionResult = controller.GetEmployeeById(1);
            var result = actionResult.Result as OkObjectResult;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var value = result.Value;
            result.Value.Should().BeEquivalentTo(firstEmployee);
        }

        [Fact]
        public void GetEmployeeById_shouldReturnBadRequest_EmployeeWithIDNotExists()
        {
            //arrange
            var employees = EmployeeData.GetSampleEmployees();
            var firstEmployee = employees[0];
            var controller = new EmployeeController(_mockRepo.Object);

            //act
            var actionResult = controller.GetEmployeeById(2);

            //assert
            var result = actionResult.Result;
            Assert.IsType<NotFoundObjectResult>(result);
        }

        private bool checkIfUserCanBeVoter(int age)
        {
            return age >= 18 ? true : false;
        }

        [Theory]
        [InlineData(18)]
        [InlineData(20)]
        public void checkIfUserCanBeVoter_true_ageGreaterThan18(int age)
        {
            //arrange
            var controller = new EmployeeController(null);

            //act
            var actual = checkIfUserCanBeVoter(age);

            //Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData(17)]
        [InlineData(15)]
        public void checkIfUserCanBeVoter_true_ageLessThan18(int age)
        {
            //arrange
            var controller = new EmployeeController(null);

            //act
            var actual = checkIfUserCanBeVoter(age);

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void CreateEmployee_CreatedStatus_PassingEmployeeObjectToCreate()
        {
            var employees = EmployeeData.GetSampleEmployees();
            var newEmployee = employees[0];
            var controller = new EmployeeController(_mockRepo.Object);
            var actionResult = controller.CreateEmployee(newEmployee);
            var result = actionResult.Result;
            Assert.IsType<CreatedAtRouteResult>(result);

        }

        
    }
}
