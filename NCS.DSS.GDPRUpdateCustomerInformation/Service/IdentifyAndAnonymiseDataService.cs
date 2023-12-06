using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Service
{
    public class IdentifyAndAnonymiseDataService : IIdentifyAndAnonymiseDataService
    {
        private readonly string _GDPRUpdateCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRUpdateCustomersStoredProcedureName");
        private readonly string _GDPRIdentifyCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRIdentifyCustomersStoredProcedureName");

        private readonly IAzureSqlDbProvider _azureSqlDbProvider;
        private readonly ILogger<IdentifyAndAnonymiseDataService> _logger;

        public IdentifyAndAnonymiseDataService(
            AzureSqlDbProvider azureSqlDbProvider,
            ILogger<IdentifyAndAnonymiseDataService> logger)
        {
            _azureSqlDbProvider = azureSqlDbProvider;
            _logger = logger;
        }

        public async Task AnonymiseData()
        {
            _azureSqlDbProvider.ExecuteStoredProcedure(_GDPRUpdateCustomersStoredProcedureName);
        }

        public async Task IdentifyCustomers()
        {
            _azureSqlDbProvider.ExecuteStoredProcedure(_GDPRIdentifyCustomersStoredProcedureName);
        }
    }
}
