using BenefitsApp.Database;

namespace BenefitsApp.Interfaces
{
    /// <summary>
    /// Represents a service that can safely generate a <see cref="BenefitsContext"/> instance connected to the database. 
    /// Having a DI-injected service provide the BenefitsContext aids with unit testing.
    /// </summary>
    public interface IBenefitsContextProvider
    {
        BenefitsContext GetContext();
    }
}
