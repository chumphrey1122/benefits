using BenefitsApp.Database;

namespace BenefitsApp.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);

        Task<Employee> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken);

        Task<Employee> CreateEmployeeAsync(string firstName, string lastName, decimal payRate);

        Task<Dependent> CreateDependentAsync(int employeeId, string firstName, string lastName);

        Task DeleteDependentAsync(int employeeId, int dependentId);
    }
}
