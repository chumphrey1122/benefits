using BenefitsApp.Database;

namespace BenefitsApp.Interfaces
{
    public interface IBenefitsContextProvider
    {
        BenefitsContext GetContext();
    }
}
