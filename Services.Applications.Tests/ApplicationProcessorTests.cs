using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Applications.Adapters;
using Services.Applications.Registry;
using Services.Applications.Validation;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Tests
{
    [TestClass]
    public class ApplicationProcessorTests
    {
        private Application _application;
        private ApplicationProcessor _processor;
        private Mock<IValidationService> _validationServiceMock;
        private Mock<IAdministratorAdapter> _administratorAdapterMock;
        private Mock<IProductRegistry> _productRegistryMock;


        [TestInitialize]
        public void TestInitialize()
        {
            _application = new Application();

            _validationServiceMock = new Mock<IValidationService>();

            _administratorAdapterMock = new Mock<IAdministratorAdapter>();

            _productRegistryMock = new Mock<IProductRegistry>();
            _productRegistryMock.Setup(p => p.GetAdministratorAdapter(_application.ProductCode))
                .Returns(_administratorAdapterMock.Object);
        }

        [TestMethod]
        public Task ApplicationCreated_when_validation_successfull()
        {
            // Setup
            _validationServiceMock.Setup(p => p.IsApplicationValid(_application))
                .Returns(true);

            // Processor
            _processor = new ApplicationProcessor(_validationServiceMock.Object, _productRegistryMock.Object);
            _processor.Process(_application);

            // Create investor should be called once
            _administratorAdapterMock.Verify(m => m.CreateInvestor(_application), Times.Once());

            return Task.CompletedTask;
        }

        [TestMethod]
        public Task ApplicationNotCreated_when_validation_failed()
        {
            // Setup
            _validationServiceMock.Setup(p => p.IsApplicationValid(_application))
                .Returns(false);

            // Processor
            _processor = new ApplicationProcessor(_validationServiceMock.Object, _productRegistryMock.Object);
            _processor.Process(_application);

            // Create investor should NOT be called
            _administratorAdapterMock.Verify(m => m.CreateInvestor(It.IsAny<Application>()), Times.Never());

            return Task.CompletedTask;
        }
    }
}
