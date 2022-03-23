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
    /// <summary>
    /// A BenefitsContextProvider that returns a dummy database suitable for use in unit tests. The database is 
    /// pre-populated with "standard" data.
    /// </summary>
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
            // Delete any existing data stored in memory...
            if (context.Dependents.Any())
                context.Dependents.RemoveRange(context.Dependents);
            if (context.Employees.Any())
                context.Employees.RemoveRange(context.Employees);

            // Create the dummy data
            // Employee 1: No dependents. No discounts
            context.Employees.Add(new Employee() { Id = 1, FirstName = "John", LastName = "Smith", PayRate = 2000m });
            // Employee 2: 3 dependents, 1 dependent discount
            context.Employees.Add(new Employee() { Id = 2, FirstName = "John", LastName = "Doe", PayRate = 2000m });
            context.Dependents.Add(new Dependent() { EmployeeId = 2, Id = 2, FirstName = "Jane", LastName = "Andersen" });
            context.Dependents.Add(new Dependent() { EmployeeId = 2, Id = 3, FirstName = "Dan", LastName = "Doe" });
            context.Dependents.Add(new Dependent() { EmployeeId = 2, Id = 4, FirstName = "Darren", LastName = "Doe" });
            // Employee 3: 1 dependent, 1 employee discount
            context.Employees.Add(new Employee() { Id = 3, FirstName = "John", LastName = "Adams", PayRate = 2000m });
            context.Dependents.Add(new Dependent() { EmployeeId = 3, Id = 5, FirstName = "George", LastName = "Washington" });
            context.SaveChanges();

            // Return the context
            return context;
        }
    }
}
