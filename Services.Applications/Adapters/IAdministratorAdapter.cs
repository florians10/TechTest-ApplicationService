using Services.Common.Abstractions.Model;

namespace Services.Applications.Adapters
{
    public interface IAdministratorAdapter
    {
        Task CreateInvestor(Application application);
    }
}
