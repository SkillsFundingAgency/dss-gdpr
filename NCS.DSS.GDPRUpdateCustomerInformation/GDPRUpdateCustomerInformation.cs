using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NCS.DSS.GDPRUpdateCustomerInformation
{
    public class GDPRUpdateCustomerInformation
    {
        [FunctionName("GDPRUpdateCustomerInformation")]
        public void Run([TimerTrigger("0 0 2 6 4 *")]TimerInfo myTimer, ILogger log)
        {
        }
    }
}
