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
using NCS.DSS.GDPRUpdateCustomerInformation.Models;
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

        public async Task<bool> DoesResourceExist(Guid customerId, string collection, Uri documentUri)
        {
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

        public async Task DeleteRecordsForCustomer(Guid customerId)
        {
            var client = DocumentDBClient.CreateDocumentClient();
            if (client == null)
                return;

            await RemoveDocumentsFromCustomer(customerId, client, "actionplans");
            await RemoveDocumentsFromCustomer(customerId, client, "actions");
            await RemoveDocumentsFromCustomer(customerId, client, "addresses");
            await RemoveDocumentsFromCustomer(customerId, client, "contacts");
            await RemoveDocumentsFromCustomer(customerId, client, "employmentprogressions");
            await RemoveDocumentsFromCustomer(customerId, client, "goals");
            await RemoveDocumentsFromCustomer(customerId, client, "webchats");

            var documentUri = DocumentDBHelper.CreateDocumentUri(customerId, "customers");
            if (await DoesResourceExist(customerId, "customers", documentUri))
            {
                try
                {
                    var response = await client.DeleteDocumentAsync(documentUri);
                }
                catch (DocumentClientException)
                {
                    return;
                }
            }
            return;
        }

        private async Task RemoveDocumentsFromCustomer(Guid customerId, DocumentClient client, string collection)
        {
            var documentIds = await GetDocumentsFromCustomer(customerId, client, collection);

            if (documentIds != null)
            {
                foreach (var documentId in documentIds)
                {
                    var documentUri = DocumentDBHelper.CreateDocumentUri((Guid)documentId.Id, collection);
                    try
                    {
                        var response = await client.DeleteDocumentAsync(documentUri);
                    }
                    catch (DocumentClientException)
                    {
                        return;
                    }
                }
            }
        }

        private async Task<List<DocumentId>> GetDocumentsFromCustomer(Guid customerId, DocumentClient client, string collection)
        {
            var collectionUri = DocumentDBHelper.CreateDocumentCollectionUri(collection);

            var documentIdsQuery = client.CreateDocumentQuery<Models.DocumentId>(collectionUri)
                .Where(so => so.CustomerId == customerId).AsDocumentQuery();

            var documentIds = new List<Models.DocumentId>();

            while (documentIdsQuery.HasMoreResults)
            {
                var response = await documentIdsQuery.ExecuteNextAsync<Models.DocumentId>();
                documentIds.AddRange(response);
            }

            return documentIds.Any() ? documentIds : null;
        }
    }

}