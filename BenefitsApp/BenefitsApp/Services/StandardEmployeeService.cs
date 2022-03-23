using BenefitsApp.Database;
using BenefitsApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BenefitsApp.Services
{
    internal class StandardEmployeeService : IEmployeeService
    {
        private IBenefitsContextProvider _benefitsContextProvider;
        private decimal _defaultPayRate;

        public StandardEmployeeService(IBenefitsContextProvider benefitsContextProvider, IConfiguration configuration)
        {
            _benefitsContextProvider = benefitsContextProvider;
            _defaultPayRate = configuration.GetValue<decimal>("DefaultPayRate");
        }

        /// <inheritdoc/>
        public async Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var context = _benefitsContextProvider.GetContext();
            var employees = await context.Employees.ToListAsync(cancellationToken);
            return employees;
        }

        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var context = _benefitsContextProvider.GetContext();
            var employee = await context.Employees.Include("Dependents").FirstOrDefaultAsync(x=>x.Id == employeeId, cancellationToken);
            return employee;
        }

        public async Task<Employee> CreateEmployeeAsync(string firstName, string lastName)
        {
            var employee = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                PayRate = _defaultPayRate,
            };
            var context = _benefitsContextProvider.GetContext();
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task<Dependent> CreateDependentAsync(int employeeId, string firstName, string lastName)
        {
            // TODO: properly handle the case that the employeeId doesn't exist, rather than relying on a SQL foreign key violation error.
            var dependent = new Dependent()
            {
                EmployeeId = employeeId,
                FirstName = firstName,
                LastName = lastName,
            };
            var context = _benefitsContextProvider.GetContext();
            context.Dependents.Add(dependent);
            await context.SaveChangesAsync();
            return dependent;
        }

        public async Task DeleteDependentAsync(int employeeId, int dependentId)
        {
            // TODO: We should really check if the employeeId exists, and if not throw an exception
            var context = _benefitsContextProvider.GetContext();
            var dependent = await context.Dependents.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Id == dependentId);
            if (dependent != null)
            {
                context.Dependents.Remove(dependent);
                await context.SaveChangesAsync();
            }

        }
    }
}
