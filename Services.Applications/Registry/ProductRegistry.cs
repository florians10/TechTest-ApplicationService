using Services.Applications.Adapters;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Registry
{
    internal class ProductRegistry : IProductRegistry
    {
        private static readonly Dictionary<ProductCode, IAdministratorAdapter> _administratorStrategies;
        private static readonly Dictionary<ProductCode, ProductConfiguration> _productConfig;

        static ProductRegistry()
        {
            _administratorStrategies = new Dictionary<ProductCode, IAdministratorAdapter>
            {
                {ProductCode.ProductOne, new AdministratorOneAdapter()},
                {ProductCode.ProductTwo, new AdministratorTwoAdapter()}
            };
            _productConfig = new Dictionary<ProductCode, ProductConfiguration> {
                { ProductCode.ProductOne, new ProductConfiguration { MinimumAge = 18, MaximumAge = 39, MinimumInitialPayment = 0.99M } },
                { ProductCode.ProductTwo, new ProductConfiguration { MinimumAge = 18, MaximumAge = 50, MinimumInitialPayment = 0.99M } }
            };
        }

        public IAdministratorAdapter GetAdministratorAdapter(ProductCode productCode) 
        {
            if(_administratorStrategies.TryGetValue(productCode, out var strategy))
            {
                return strategy;
            }
            throw new NotSupportedException();
        }

        public ProductConfiguration GetProductConfiguration(ProductCode productCode)
        {
            if (_productConfig.TryGetValue(productCode, out var productConfiguration))
            {
                return productConfiguration;
            }
            throw new NotSupportedException();
        }
    }
}
