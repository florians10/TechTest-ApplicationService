using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Adapters
{
    internal class AdministratorOneAdapter : IAdministratorAdapter
    {
        private IAdministrationService _administrationService;

        public AdministratorOneAdapter()
        {
        }

        public AdministratorOneAdapter(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }


        public Task CreateInvestor(Application application)
        {
            var request = ToCreateInvestorRequest(application);

            try
            {
                var response = _administrationService.CreateInvestor(request);
                ProcessResponse(response, application);
            }
            catch (Exception e)
            { }

            return Task.CompletedTask;

        }

        private CreateInvestorRequest ToCreateInvestorRequest(Application application)
        {
            return new CreateInvestorRequest
            {
                Reference = application.Id.ToString(),
                FirstName = application.Applicant.Forename,
                LastName = application.Applicant.Surname,
                //etc
            };
        }

        private void ProcessResponse(CreateInvestorResponse response, Application application)
        {
            new InvestorCreated(application.Applicant.Id, response.InvestorId);
            new AccountCreated(response.InvestorId, application.ProductCode, response.AccountId);
        }
    }
}
