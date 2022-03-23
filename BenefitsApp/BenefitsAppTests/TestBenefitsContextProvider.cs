using BenefitsApp.Database;
using BenefitsApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenefitsAppTests
{
    internal class TestBenefitsContextProvider : IBenefitsContextProvider
    {
        private class TestBenefitsContext : BenefitsContext
        {
            public TestBenefitsContext() : base("")
            {
            }

            protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseInMemoryDatabase("test");
        }

        public BenefitsContext GetContext()
        {
            var context = new TestBenefitsContext();
            // Create the dummy data
            context.Employees.Add(new Employee() { Id = 1, FirstName = "User", LastName = "One", PayRate = 2000m });
            context.Dependents.Add(new Dependent() { EmployeeId = 1, Id = 1, FirstName = "Dependent", LastName = "One" });
            context.SaveChanges();

            // Return the context
            return context;
        }
    }
}
