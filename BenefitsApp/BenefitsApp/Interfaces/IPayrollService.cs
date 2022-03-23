using BenefitsApp.Models;

namespace BenefitsApp.Interfaces
{
    public interface IPayrollService
    {
        Task<EmployeePayrollInformation> GetPayroll(int employeeId, CancellationToken cancellationToken);
    }
}
