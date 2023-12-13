using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Service
{
    public class IdentifyAndAnonymiseDataService : IIdentifyAndAnonymiseDataService
    {
        private readonly string _GDPRUpdateCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRUpdateCustomersStoredProcedureName");
        private readonly string _GDPRIdentifyCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRIdentifyCustomersStoredProcedureName");

        private readonly IAzureSqlDbProvider _azureSqlDbProvider;
        private readonly ILogger<IIdentifyAndAnonymiseDataService> _logger;
        private readonly IDocumentDBProvider _documentDbProvider;

        public IdentifyAndAnonymiseDataService(
            IDocumentDBProvider documentDbProvider,
            IAzureSqlDbProvider azureSqlDbProvider,
            ILogger<IIdentifyAndAnonymiseDataService> logger)
        {
            _azureSqlDbProvider = azureSqlDbProvider;
            _documentDbProvider = documentDbProvider;
            _logger = logger;
        }

        public async Task AnonymiseData()
        {
            await _azureSqlDbProvider.ExecuteStoredProcedureAsync(_GDPRUpdateCustomersStoredProcedureName);
        }

        public async Task DeleteCustomersfromCosmos()
        {
            var idList = await _azureSqlDbProvider.ExecuteStoredProcedureAsync(_GDPRIdentifyCustomersStoredProcedureName);
            if (idList != null) {
                foreach (var id in idList)
                {
                    await _documentDbProvider.DeleteRecordsForCustomer(id);
                }
            }
        }
    }
}
