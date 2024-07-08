using Services.Applications.Adapters;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Registry
{
    public interface IProductRegistry
    {
        IAdministratorAdapter GetAdministratorAdapter(ProductCode productCode);

        ProductConfiguration GetProductConfiguration(ProductCode productCode);
    }
}
