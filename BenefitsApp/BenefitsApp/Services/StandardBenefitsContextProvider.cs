using BenefitsApp.Database;
using BenefitsApp.Interfaces;

namespace BenefitsApp.Services
{
    public class StandardBenefitsContextProvider : IBenefitsContextProvider
    {
        private readonly string _connectionString;

        public StandardBenefitsContextProvider(IConfiguration configuration)
        {
            _connectionString = configuration["DatabaseConnectionString"];
        }

        public BenefitsContext GetContext()
        {
            return new BenefitsContext(_connectionString);
        }
    }
}
