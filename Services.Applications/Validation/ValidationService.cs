using Services.Applications.Registry;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Validation
{
    internal class ValidationService : IValidationService
    {
        private IProductRegistry _productRegistry;

        public ValidationService(IProductRegistry productRegistry)
        {
            _productRegistry = productRegistry;
        }

        public bool IsApplicationValid(Application application)
        {
            var productConfig = _productRegistry.GetProductConfiguration(application.ProductCode);

            // Age restriction check
            var dateTimeNow = DateTime.Now;
            if (application.Applicant.DateOfBirth.AddYears(productConfig.MinimumAge) > DateOnly.FromDateTime(dateTimeNow))
            {
                return false;
            }
            if (application.Applicant.DateOfBirth.AddYears(productConfig.MaximumAge) < DateOnly.FromDateTime(dateTimeNow))
            {
                return false;
            }

            // Minimum payment restriction
            if(productConfig.MinimumInitialPayment > application.Payment.Amount.Amount)
            {
                return false;
            }

            // Kyc check - if new user

            // Verified User
            if(!application.Applicant.IsVerified.HasValue || application.Applicant.IsVerified.Value == false)
            {
                return false;
            }

            return true;
        }
    }
}
