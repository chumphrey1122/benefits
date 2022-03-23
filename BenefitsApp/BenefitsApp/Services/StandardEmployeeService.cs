using BenefitsApp.Database;
using BenefitsApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BenefitsApp.Services
{
    internal class StandardEmployeeService : IEmployeeService
    {
        private IBenefitsContextProvider _benefitsContextProvider;

        public StandardEmployeeService(IBenefitsContextProvider benefitsContextProvider)
        {
            _benefitsContextProvider = benefitsContextProvider;
        }
        public async Task<List<Employee>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var context = _benefitsContextProvider.GetContext();
            var employees = await context.Employees.ToListAsync(cancellationToken);
            return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var context = _benefitsContextProvider.GetContext();
            var employee = await context.Employees.Include("Dependents").FirstOrDefaultAsync(x=>x.Id == employeeId, cancellationToken);
            return employee;
        }
        public async Task<Employee> CreateEmployeeAsync(string firstName, string lastName, decimal payRate)
        {
            var employee = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                PayRate = payRate
            };
            var context = _benefitsContextProvider.GetContext();
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task<Dependent> CreateDependentAsync(int employeeId, string firstName, string lastName)
        {
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
