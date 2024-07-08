using Services.Common.Abstractions.Model;

namespace Services.Applications.Validation
{
    public interface IValidationService
    {
        bool IsApplicationValid(Application application);
    }
}
