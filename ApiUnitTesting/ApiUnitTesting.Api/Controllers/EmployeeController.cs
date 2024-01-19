using ApiUnitTesting.Api.Model;
using ApiUnitTesting.Api.Repo;
using Microsoft.AspNetCore.Mvc;

namespace ApiUnitTesting.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController: ControllerBase
    {
        private IGenericRepository<Employee> _repository;

        public EmployeeController(IGenericRepository<Employee> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAllEmployee")]
        public ActionResult<IEnumerable<Employee>> GetEmployee()
        {
            var employees = _repository.GetAll();
            return Ok(employees);
        }

        [HttpGet("GetEmployeeById/{id}")]
        public ActionResult<Employee> GetEmployeeById(long id)
        {
            Employee employee = _repository.GetById(id);
            if (employee == null)
            {
                return NotFound("The Employee record couldn't be found.");
            }
            return Ok(employee);
        }

        [HttpPost("CreateEmployee")]
        public ActionResult<Employee> CreateEmployee(Employee employee)
        {
            if (employee == null) {
                return BadRequest("Employee is null");
            }
            //if (employee.EmployeeId == null || employee.EmployeeId == 0)
            //{
            //    return BadRequest("EmployeeId is not supplied");
            //}
            _repository.Insert(employee);
            return CreatedAtRoute("GetEmployeeById", new { Id = employee.EmployeeId }, employee);
        }

        [HttpPut]
        public IActionResult Put(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee is null");
            }
            _repository.Update(employee);
            return NoContent();
        }

        private bool checkIfUserCanBeVoter(int age)
        {
            return (age >= 18) ? true : false;
        }
    }
}
