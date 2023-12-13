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

        [FunctionName("GDPRIdentifyCustomers")]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            await _IdentifyAndAnonymiseDataService.AnonymiseData();

            await _IdentifyAndAnonymiseDataService.DeleteCustomersfromCosmos();
        }
    }
}
