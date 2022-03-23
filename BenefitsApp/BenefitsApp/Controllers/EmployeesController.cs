using BenefitsApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace BenefitsApp.Controllers
{
    /// <summary>
    /// This controller allows management of the list of employees and their dependents. 
    /// TODO: There are some missing endpoints: 
    /// PUT {employeeId} - to update the employee's name
    /// PUT {employeeId}/dependents/{dependentId} - to update the dependent's name
    /// DELETE {employeeId} - to delete the employee
    /// </summary>
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

        /// <summary>
        /// Get a list of all employees for the company, but not their dependents
        /// 
        /// TODO: Support filtering and sorting. We would pass in these options as query string parameters (e.g. ?page=1&orderBy=firstName)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<Database.Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                // TODO: We should not directly return the Database objects on the API request, but instead create a dedicated Data Transfer Object (class that 
                // has just the properties we wish to expose and that gets populated from the database object). This prevents accidentally exposing something
                // on the API that's supposed to be private in the database.
                return this.Ok(await _employeeService.GetEmployeesAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }

        /// <summary>
        /// Get the detailed information for an employee, including their dependents
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Database.Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Create a new employee with a given name
        /// 
        /// TODO: We should probably have the employer pass in the EmployeeId rather than have us automatically generate it when
        /// we write the user to the database. This would prevent accidentally adding the same user multiple times, and would 
        /// allow us to distinguish between multiple employees that have the same name.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Database.Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("")]
        public async Task<IActionResult> CreateNewEmployee(CreateEmployeeModel employee, CancellationToken cancellationToken)
        {
            try
            {
                return this.Ok(await _employeeService.CreateEmployeeAsync(employee.FirstName, employee.LastName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }

        /// <summary>
        /// Create a new dependent with a given name
        /// 
        /// TODO: We should explicitly check if the employeeId exists and if not, return a 404 error. Instead, what currently happens is we get a SQL
        /// Foreign Key violation constraint error exception, which gets caught in our general catch block, and we return a 500 error code.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dependent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Database.Dependent), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Delete a given employee's dependent. If the dependent doesn't exist, return OK
        /// 
        /// TODO: If the employee doesn't exist, we should probably return a 404 Not found error.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="dependentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
