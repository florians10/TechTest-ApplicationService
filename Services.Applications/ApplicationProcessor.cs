using Services.Applications.Registry;
using Services.Applications.Validation;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications;

public class ApplicationProcessor : IApplicationProcessor
{
    private IValidationService _validationService;
    private IProductRegistry _productRegistry;

    public ApplicationProcessor(IValidationService validationService, IProductRegistry productRegistry)
    {
        _validationService = validationService;
        _productRegistry = productRegistry;
    }

    public Task Process(Application application)
    {
        if (_validationService.IsApplicationValid(application))
        {
            var administratorService = _productRegistry.GetAdministratorAdapter(application.ProductCode);
            administratorService.CreateInvestor(application);
            new ApplicationCompleted(application.Id);
        }
        return Task.CompletedTask;
    }
}