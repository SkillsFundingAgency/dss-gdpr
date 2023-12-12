using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Client;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper;
using Newtonsoft.Json.Linq;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public class DocumentDBProvider : IDocumentDBProvider
    {

        private string _customerJson;

        public string GetCustomerJson()
        {
            return _customerJson;
        }

        public async Task<bool> DoesCustomerResourceExist(Guid customerId)
        {
            var documentUri = DocumentDBHelper.CreateCustomerDocumentUri(customerId);

            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
                return false;
            try
            {
                var response = await client.ReadDocumentAsync(documentUri);
                if (response.Resource != null)
                {
                    _customerJson = response.Resource.ToString();
                    return true;
                }
            }
            catch (DocumentClientException)
            {
                return false;
            }

            return false;
        }

        public async Task<Models.Customer> GetCustomerByIdAsync(Guid customerId)
        {
            var documentUri = DocumentDBHelper.CreateCustomerDocumentUri(customerId);

            var client = DocumentDBClient.CreateDocumentClient();

            if (client == null)
                return null;

            try
            {
                var response = await client.ReadDocumentAsync(documentUri);
                if (response.Resource != null)
                    return (dynamic)response.Resource;
            }
            catch (DocumentClientException)
            {
                return null;
            }

            return null;
        }
    }

}