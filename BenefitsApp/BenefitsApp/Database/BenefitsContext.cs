using Microsoft.EntityFrameworkCore;

namespace BenefitsApp.Database
{
    public class BenefitsContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Dependent> Dependents { get; set; }
        private string _connectionString;

        public BenefitsContext (string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_connectionString);
            
    }

}
