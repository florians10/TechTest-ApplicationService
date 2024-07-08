using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Applications.Registry;
using Services.Applications.Validation;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Tests.Validation
{
    [TestClass]
    public class ValidationServiceTests
    {
        private IValidationService _validationService;

        [TestInitialize]
        public void TestInitialize()
        {
            var productRegistry = new ProductRegistry();
            _validationService = new ValidationService(productRegistry);
        }

        #region ProductOne

        [DataTestMethod]
        [DataRow(22)]
        [DataRow(38)]
        public void ProductOne_ApplicationSuccessful(int age)
        {
            var application = BuildApplication(ProductCode.ProductOne, GetDateOfBirthFromAgeAsDateOnly(age), true, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsTrue(response);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(17)]
        public void ProductOne_ApplicationFailed_is_published_when_Applicant_is_too_young(int age)
        {
            var application = BuildApplication(ProductCode.ProductOne, GetDateOfBirthFromAgeAsDateOnly(age), true, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        [DataTestMethod]
        [DataRow(50)]
        [DataRow(40)]
        public void ProductOne_ApplicationFailed_is_published_when_Applicant_is_too_old(int age)
        {
            var application = BuildApplication(ProductCode.ProductOne, GetDateOfBirthFromAgeAsDateOnly(age), true, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void ProductOne_ApplicationFailed_is_published_when_InitialPayment_not_meeting_minimum()
        {
            var application = BuildApplication(ProductCode.ProductOne, GetDateOfBirthFromAgeAsDateOnly(32), true, 0.5M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void ProductOne_ApplicationFailed_is_published_when_Applicant_is_not_verified()
        {
            var application = BuildApplication(ProductCode.ProductOne, GetDateOfBirthFromAgeAsDateOnly(32), false, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        #endregion

        #region ProductTwo

        [DataTestMethod]
        [DataRow(22)]
        [DataRow(49)]
        public void ProductTwo_ApplicationSuccessful(int age)
        {
            var application = BuildApplication(ProductCode.ProductTwo, GetDateOfBirthFromAgeAsDateOnly(age), true, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsTrue(response);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(17)]
        public void ProductTwo_ApplicationFailed_is_published_when_Applicant_is_too_young(int age)
        {
            var application = BuildApplication(ProductCode.ProductTwo, GetDateOfBirthFromAgeAsDateOnly(age), true, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        [DataTestMethod]
        [DataRow(70)]
        [DataRow(51)]
        public void ProductTwo_ApplicationFailed_is_published_when_Applicant_is_too_old(int age)
        {
            var application = BuildApplication(ProductCode.ProductTwo, GetDateOfBirthFromAgeAsDateOnly(age), true, 10M);

            // Processor
            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void ProductTwo_ApplicationFailed_is_published_when_InitialPayment_not_meeting_minimum()
        {
            var application = BuildApplication(ProductCode.ProductTwo, GetDateOfBirthFromAgeAsDateOnly(32), true, 0.5M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void ProductTwo_ApplicationFailed_is_published_when_Applicant_is_not_verified()
        {
            var application = BuildApplication(ProductCode.ProductTwo, GetDateOfBirthFromAgeAsDateOnly(32), false, 10M);

            var response = _validationService.IsApplicationValid(application);
            Assert.IsFalse(response);
        }

        #endregion


        private Application BuildApplication(ProductCode productCode, DateOnly dateOfBirth, bool isVerified, decimal initialAmount)
        {
            return new Application
            {
                ProductCode = productCode,
                Applicant = new User
                {
                    DateOfBirth = dateOfBirth,
                    IsVerified = isVerified
                },
                Payment = new Payment(new BankAccount(), new Money("GBP", initialAmount))
            };
        }

        private DateOnly GetDateOfBirthFromAgeAsDateOnly(int age)
        {
            return DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-age));
        }

    }
}
