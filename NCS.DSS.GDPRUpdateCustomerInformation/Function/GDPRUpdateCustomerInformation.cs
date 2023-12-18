using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.Service;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Function
{
    public class GDPRUpdateCustomerInformation
    {
        private readonly IIdentifyAndAnonymiseDataService _IdentifyAndAnonymiseDataService;
        private readonly ILogger<GDPRUpdateCustomerInformation> _logger;

        public GDPRUpdateCustomerInformation(IIdentifyAndAnonymiseDataService IdentifyAndAnonymiseDataService, ILogger<GDPRUpdateCustomerInformation> logger)
        {
            _IdentifyAndAnonymiseDataService = IdentifyAndAnonymiseDataService;
            _logger = logger;
        }

        //string input only necessary for manual trigger: https://github.com/Azure/azure-functions-vs-build-sdk/issues/168#issuecomment-378913905
        [FunctionName("GDPRUpdateCustomerInformation")]
        [NoAutomaticTrigger]
        [Singleton]
        public async Task Run(string input, ILogger log)
        {
            log.LogInformation($"Function executed at: {DateTime.UtcNow}");

            var customerIds = await _IdentifyAndAnonymiseDataService.ReturnCustomerIds();

            if (customerIds.Count.Equals(0)) {
                log.LogInformation("No customers fall outside of GDPR compliance");
                return;
            }

            log.LogInformation($"{customerIds.Count} customers identified that fall outside of GDPR compliance");

            log.LogInformation("Attempting to redact data from SQL");

            await _IdentifyAndAnonymiseDataService.AnonymiseData();

            log.LogInformation("Successfully redacted customer information from SQL");

            log.LogInformation("Attempting to delete related records from CosmosDB");

            await _IdentifyAndAnonymiseDataService.DeleteCustomersFromCosmos(customerIds);

            log.LogInformation("Successfully deleted related records from CosmosDB");

            log.LogInformation("All GPDR function tasks completed successfully");
        }
    }
}
