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
        private readonly IResourceHelper _resourceHelper;
        private readonly IDocumentDBProvider _documentDbProvider;

        public IdentifyAndAnonymiseDataService(
            IResourceHelper resourceHelper,
            IDocumentDBProvider documentDbProvider,
            IAzureSqlDbProvider azureSqlDbProvider,
            ILogger<IIdentifyAndAnonymiseDataService> logger)
        {
            _resourceHelper = resourceHelper;
            _azureSqlDbProvider = azureSqlDbProvider;
            _documentDbProvider = documentDbProvider;
            _logger = logger;
        }

        public async Task AnonymiseData()
        {
            await _azureSqlDbProvider.ExecuteStoredProcedureAsync(_GDPRUpdateCustomersStoredProcedureName);
        }

        public async Task IdentifyCustomers()
        {
            var idList = await _azureSqlDbProvider.ExecuteStoredProcedureAsync(_GDPRIdentifyCustomersStoredProcedureName);
            var id = idList.First();
            var doesCustomerExist = await _resourceHelper.DoesCustomerExist(id);

            var customer = await _documentDbProvider.GetCustomerByIdAsync(id);

            _logger.LogInformation("Return of doesCustomerExist: ", doesCustomerExist);
            //do cosmos changes
        }
    }
}
