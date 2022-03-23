using BenefitsApp.Models;

namespace BenefitsApp.Interfaces
{
    /// <summary>
    /// Represents a service that can obtain payroll-related information for a user
    /// </summary>
    public interface IPayrollService
    {
        Task<EmployeePayrollInformation> GetPayroll(int employeeId, CancellationToken cancellationToken);
    }
}
