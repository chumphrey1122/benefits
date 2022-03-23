using BenefitsApp.Database;

namespace BenefitsApp.Interfaces
{
    /// <summary>
    /// Represents a service that allows management of employees and their dependents
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Returns a list of Employees, but doesn't populate their Dependents
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets detailed Employee information, including Dependents
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Employee> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken);

        Task<Employee> CreateEmployeeAsync(string firstName, string lastName);

        Task<Dependent> CreateDependentAsync(int employeeId, string firstName, string lastName);

        Task DeleteDependentAsync(int employeeId, int dependentId);
    }
}
