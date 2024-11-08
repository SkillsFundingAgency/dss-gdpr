using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.Service;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Function
{
    public class GDPRUpdateCustomerInformation
    {
        private readonly IIdentifyAndAnonymiseDataService _identifyAndAnonymiseDataService;
        private readonly ILogger<GDPRUpdateCustomerInformation> _logger;

        public GDPRUpdateCustomerInformation(IIdentifyAndAnonymiseDataService identifyAndAnonymiseDataService, ILogger<GDPRUpdateCustomerInformation> logger)
        {
            _identifyAndAnonymiseDataService = identifyAndAnonymiseDataService;
            _logger = logger;
        }

        [Function(nameof(GDPRUpdateCustomerInformation))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(GDPRUpdateCustomerInformation)} has been invoked");
            _logger.LogInformation("Attempting to retrieve list of customer IDs");

            try
            {
                List<Guid> customerIds = await _identifyAndAnonymiseDataService.ReturnCustomerIds();

                if (customerIds.Count.Equals(0))
                {
                    _logger.LogInformation("All Customers are GDPR compliant.");
                    return new EmptyResult();
                }

                _logger.LogInformation($"A total of {customerIds.Count().ToString()} Customer IDs have been identified as being non-compliant with GDPR.");

                _logger.LogInformation("Attempting to anonymise data from SQL DB");

                await _identifyAndAnonymiseDataService.AnonymiseData();

                _logger.LogInformation("Successfully anonymised customer information from SQL DB");

                _logger.LogInformation("Attempting to delete related documents from Cosmos DB");

                await _identifyAndAnonymiseDataService.DeleteCustomersFromCosmos(customerIds);

                _logger.LogInformation("Successfully deleted related documents from Cosmos DB");

                _logger.LogInformation($"{nameof(GDPRUpdateCustomerInformation)} has finished invocation successfully");

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GDPRUpdateCustomerInformation)} has failed to invoke. Error: {ex.Message}");
                throw;
            }
        }
    }
}
