using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenefitsAppTests
{
    internal static class Configuration
    {
        // For testing, to make round numbers, we'll use 25 pay periods per year, not 26
        public static IConfiguration Default { get; } =
            new ConfigurationBuilder()
                     .AddInMemoryCollection(new Dictionary<string, string>
                        {
                            {"DefaultPayRate", "2000"},
                            {"PayPeriodsPerYear", "25" },
                            {"BaseBenefitsCost", "1000" },
                            {"BaseDependentBenefitCost", "500" },
                            {"DiscountRate",  "0.1" }
                        })
                     .Build();
    }
}
