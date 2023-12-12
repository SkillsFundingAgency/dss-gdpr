using System;
using System.Threading.Tasks;
using DFC.JSON.Standard;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper
{
    public class ResourceHelper : IResourceHelper
    {

        private readonly IDocumentDBProvider _documentDbProvider;
        private readonly IJsonHelper _jsonHelper;

        public ResourceHelper(IDocumentDBProvider documentDbProvider, IJsonHelper jsonHelper)
        {
            _documentDbProvider = documentDbProvider;
            _jsonHelper = jsonHelper;
        }

        public async Task<bool> DoesCustomerExist(Guid customerId)
        {
            return await _documentDbProvider.DoesCustomerResourceExist(customerId);
        }

        public bool IsCustomerReadOnly()
        {
            var customerJson = _documentDbProvider.GetCustomerJson();

            if (string.IsNullOrWhiteSpace(customerJson))
                return false;

            var dateOfTermination = _jsonHelper.GetValue(customerJson, "DateOfTermination");

            return !string.IsNullOrWhiteSpace(dateOfTermination);
        }

    }
}
