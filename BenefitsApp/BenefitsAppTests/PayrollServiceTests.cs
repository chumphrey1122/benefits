using BenefitsApp.Database;
using BenefitsApp.Interfaces;
using BenefitsApp.Services;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace BenefitsAppTests
{
    public class Tests
    {
        private IPayrollService _payrollService;
        private int _payPeriodsPerYear;

        [SetUp]
        public void Setup()
        {
            var configuration = Configuration.Default;
            _payPeriodsPerYear = configuration.GetValue<int>("PayPeriodsPerYear");
            _payrollService = new StandardPayrollService(configuration, new TestBenefitsContextProvider());
        }

        /// <summary>
        /// Test the payroll calculation. Note: To prevent rounding errors, we are 
        /// setting there to be 25, not 26 paychecks per year for these tests.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="netYearlyPay"></param>
        [TestCase(1, 49000)]
        [TestCase(2, 47550)]
        [TestCase(3, 48600)]
        public void TestPayroll(int id, decimal netYearlyPay)
        {
            // Set up the tests
            var payroll = _payrollService.GetPayroll(id, CancellationToken.None).Result;

            Assert.AreEqual(netYearlyPay, payroll.NetPay * _payPeriodsPerYear);
        }
    }
}