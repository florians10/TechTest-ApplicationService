using Services.AdministratorTwo.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Adapters
{
    internal class AdministratorTwoAdapter : IAdministratorAdapter
    {
        private IAdministrationService _administrationService;

        public AdministratorTwoAdapter()
        {
        }
        public AdministratorTwoAdapter(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        public Task CreateInvestor(Application application)
        {
            // 1. Create investor
            var createInvestorResponse = _administrationService.CreateInvestorAsync(application.Applicant).Result;
            new InvestorCreated(application.Applicant.Id, createInvestorResponse.Value.ToString());


            // 2. Create account
            var createAccountResponse = _administrationService.CreateAccountAsync(createInvestorResponse.Value, application.ProductCode).Result;
            new AccountCreated(createInvestorResponse.Value.ToString(), application.ProductCode, createAccountResponse.Value.ToString());

            // 3. Process payment
            _administrationService.ProcessPaymentAsync(createAccountResponse.Value, application.Payment);

            return Task.CompletedTask;

        }
    }
}
