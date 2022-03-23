using BenefitsApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace BenefitsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IEmployeeService _employeeService;
        private ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _employeeService.GetEmployeesAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> Get([FromRoute] int employeeId, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(employeeId, cancellationToken);
                if (employee == null)
                    return NotFound();
                return this.Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }


        [HttpPost("")]
        public async Task<IActionResult> CreateNewEmployee(CreateEmployeeModel employee, CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _employeeService.CreateEmployeeAsync(employee.FirstName, employee.LastName, 2000m));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }

        [HttpPost("{employeeId}/dependents")]
        public async Task<IActionResult> CreateNewDependent([FromRoute] int employeeId, [FromBody] CreateDependentModel dependent, CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _employeeService.CreateDependentAsync(employeeId, dependent.FirstName, dependent.LastName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }

        [HttpDelete("{employeeId}/dependents/{dependentId}")]
        public async Task<IActionResult> DeleteDependent([FromRoute] int employeeId, [FromRoute] int dependentId, CancellationToken cancellationToken)
        {
            try
            {
                await _employeeService.DeleteDependentAsync(employeeId, dependentId);
                return this.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }

        #region Data Transfer Objects
        [DataContract]
        public class CreateEmployeeModel
        {
            [DataMember(Name = "firstName")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastName")]
            public string LastName { get; set; }
        }

        [DataContract]
        public class CreateDependentModel
        {
            [DataMember(Name = "firstName")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastName")]
            public string LastName { get; set; }
        }

        #endregion

    }
}
