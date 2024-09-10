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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation($"Function executed at: {DateTime.UtcNow}");

                var customerIds = await _identifyAndAnonymiseDataService.ReturnCustomerIds();

                if (customerIds.Count.Equals(0))
                {
                    _logger.LogInformation("No customers fall outside of GDPR compliance");
                    return new EmptyResult();
                }

                _logger.LogInformation(
                    "{CustomerIds} customers identified that fall outside of GDPR compliance", customerIds.Count);

                _logger.LogInformation("Attempting to redact data from SQL");

                await _identifyAndAnonymiseDataService.AnonymiseData();

                _logger.LogInformation("Successfully redacted customer information from SQL");

                _logger.LogInformation("Attempting to delete related records from CosmosDB");

                await _identifyAndAnonymiseDataService.DeleteCustomersFromCosmos(customerIds);

                _logger.LogInformation("Successfully deleted related records from CosmosDB");

                _logger.LogInformation("All GPDR function tasks completed successfully");

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("The function has failed: {ex.Message}", ex);
                throw;
            }
        }
    }
}
