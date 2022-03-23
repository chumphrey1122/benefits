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

        [SetUp]
        public void Setup()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"DefaultPayRate", "2000"},
                {"PayPeriodsPerYear", "26" },
                {"BaseBenefitsCost", "1000" },
                {"BaseDependentBenefitCost", "500" },
                {"DiscountRate",  "0.1" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _payrollService = new StandardPayrollService(configuration, new TestBenefitsContextProvider());
        }

        [TestCase(1, 50500)]
        public void TestPayroll(int id, decimal netPay)
        {
            // Set up the tests
            var payroll = _payrollService.GetPayroll(id, CancellationToken.None).Result;

            Assert.AreEqual(payroll.NetPay*26, netPay);
        }
    }
}