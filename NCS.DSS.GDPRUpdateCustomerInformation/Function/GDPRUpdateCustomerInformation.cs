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

        [FunctionName("GDPRUpdateCustomerInformation")]
        [NoAutomaticTrigger]
        [Singleton]
        public async Task Run(string input, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
            //await _IdentifyAndAnonymiseDataService.AnonymiseData();

            //await _IdentifyAndAnonymiseDataService.DeleteCustomersFromCosmos();

            var list = await _IdentifyAndAnonymiseDataService.ReturnLisOfCustomerIds();

            log.LogInformation(list.Count.ToString());
        }
    }
}
