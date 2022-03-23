using BenefitsApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BenefitsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private IPayrollService _payrollService;

        public PayrollController (IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet("employees/{employeeId}")]
        public async Task<IActionResult> Get([FromRoute] int employeeId, CancellationToken cancellationToken)
        {
            var payrollInfo = await _payrollService.GetPayroll(employeeId, cancellationToken);
            if (payrollInfo == null)
                return NotFound();
            return this.Ok(payrollInfo);
        }
        
    }
}
