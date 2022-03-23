using BenefitsApp.Database;
using BenefitsApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using BenefitsApp.Models;

namespace BenefitsApp.Services
{
    public class StandardPayrollService : IPayrollService
    {
        private IBenefitsContextProvider _benefitsContextProvider;
        private int _payPeriodsPerYear;
        private decimal _baseBenefitsCost;
        private decimal _baseDependentBenefitsCost;
        private decimal _discountRate;

        public StandardPayrollService(IConfiguration configuration, IBenefitsContextProvider benefitsContextProvider)
        {
            _benefitsContextProvider = benefitsContextProvider;
            _payPeriodsPerYear = configuration.GetValue<int>("PayPeriodsPerYear");
            _baseBenefitsCost = configuration.GetValue<decimal>("BaseBenefitsCost") / _payPeriodsPerYear;
            _baseDependentBenefitsCost = configuration.GetValue<decimal>("BaseDependentBenefitCost") / _payPeriodsPerYear;
            _discountRate = configuration.GetValue<decimal>("DiscountRate");
        }

        public async Task<EmployeePayrollInformation> GetPayroll(int employeeId, CancellationToken cancellationToken)
        {
            var context = _benefitsContextProvider.GetContext();
            // TODO: We should use a SQL view to get this data so we don't need to get the full names of all dependents and employees (reduces the data sent back).
            var employee = await context.Employees.Include("Dependents").FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);
            if (employee == null)
                return null; // TODO: should we return null in this case, or throw an exception?
            return this.GetPayroll(employee);
        }

        private EmployeePayrollInformation GetPayroll(Employee employee)
        {
            var numDependents = employee.Dependents.Count();
            var numDependentsWithDiscount = employee.Dependents.Count(dep => dep.LastName.ToLower().StartsWith("a"));

            return new EmployeePayrollInformation()
            {
                PayPerPeriod = employee.PayRate,
                NumDependents = numDependents,
                DependentCost = numDependents * _baseDependentBenefitsCost,
                DependentDiscount = numDependentsWithDiscount * _baseDependentBenefitsCost * _discountRate,
                BaseBenefitsCost = _baseBenefitsCost,
                BaseDiscount = employee.LastName.ToLower().StartsWith("a") ? _baseBenefitsCost * _discountRate : 0,
            };
        }
    }
}
