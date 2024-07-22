using FakeItEasy;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.Service;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Tests
{
    public class GDPRUpdateCustomerInformationTests
    {
        private readonly IIdentifyAndAnonymiseDataService _fakeDataService;
        private readonly ILogger<Function.GDPRUpdateCustomerInformation> _fakeLogger;
        private readonly Function.GDPRUpdateCustomerInformation _function;

        public GDPRUpdateCustomerInformationTests()
        {
            _fakeDataService = A.Fake<IIdentifyAndAnonymiseDataService>();
            _fakeLogger = A.Fake<ILogger<Function.GDPRUpdateCustomerInformation>>();
            _function = new Function.GDPRUpdateCustomerInformation(_fakeDataService);
        }

        [Fact]
        public async Task Run_NoCustomers_NoOperationsPerformed()
        {
            // Arrange
            A.CallTo(() => _fakeDataService.ReturnCustomerIds()).Returns(Task.FromResult(new List<Guid>()));

            // Act
            await _function.Run(string.Empty, _fakeLogger);

            // Assert
            A.CallTo(() => _fakeDataService.AnonymiseData()).MustNotHaveHappened();
            A.CallTo(() => _fakeDataService.DeleteCustomersFromCosmos(A<List<Guid>>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task Run_CustomersExist_OperationsPerformed()
        {
            // Arrange
            var customerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            A.CallTo(() => _fakeDataService.ReturnCustomerIds()).Returns(Task.FromResult(customerIds));

            // Act
            await _function.Run(string.Empty, _fakeLogger);

            // Assert
            A.CallTo(() => _fakeDataService.AnonymiseData()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _fakeDataService.DeleteCustomersFromCosmos(customerIds)).MustHaveHappenedOnceExactly();
        }
    }
}