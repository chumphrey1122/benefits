using BenefitsApp.Interfaces;
using BenefitsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BenefitsApp.Controllers
{
    /// <summary>
    /// This controller allows us to check the payroll information for a particular employee
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private IPayrollService _payrollService;
        private ILogger<PayrollController> _logger;

        public PayrollController (IPayrollService payrollService, ILogger<PayrollController> logger)
        {
            _payrollService = payrollService;
            _logger = logger;
        }

        /// <summary>
        /// Get the employee's payroll information, including the benefits calculation.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(EmployeePayrollInformation), StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("employees/{employeeId}")]
        public async Task<IActionResult> Get([FromRoute] int employeeId, CancellationToken cancellationToken)
        {
            try
            {
                var payrollInfo = await _payrollService.GetPayroll(employeeId, cancellationToken);
                if (payrollInfo == null)
                    return NotFound();
                return this.Ok(payrollInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return this.Problem();
            }
        }
        
    }
}
