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


            var plansTask = RemoveDocumentsFromCustomer(customerId, client, "actionplans");
            var actionsTask = RemoveDocumentsFromCustomer(customerId, client, "actions");
            var addressesTask = RemoveDocumentsFromCustomer(customerId, client, "addresses");
            var contactsTask = RemoveDocumentsFromCustomer(customerId, client, "contacts");
            var employmentProgressionsTask = RemoveDocumentsFromCustomer(customerId, client, "employmentprogressions");
            var goalsTask = RemoveDocumentsFromCustomer(customerId, client, "goals");
            var webchatsTask = RemoveDocumentsFromCustomer(customerId, client, "webchats");
            var digitalIdentitiesTask = RemoveDocumentsFromCustomer(customerId, client, "digitalidentities");
            var diverityDetailsTask = RemoveDocumentsFromCustomer(customerId, client, "diveritydetails");
            var learningProgressionsTask = RemoveDocumentsFromCustomer(customerId, client, "learningprogressions");
            var outcomesTask = RemoveDocumentsFromCustomer(customerId, client, "outcomes");
            var sessionsTask = RemoveDocumentsFromCustomer(customerId, client, "sessions");
            var subscriptionsTask = RemoveDocumentsFromCustomer(customerId, client, "subscriptions");
            var transfersTask = RemoveDocumentsFromCustomer(customerId, client, "transfers");

            await Task.WhenAll(plansTask, actionsTask, addressesTask, contactsTask, employmentProgressionsTask, 
                goalsTask, webchatsTask, digitalIdentitiesTask, diverityDetailsTask, learningProgressionsTask, 
                outcomesTask, sessionsTask, subscriptionsTask, transfersTask);

            var documentUri = DocumentDBHelper.CreateDocumentUri(customerId, "customers");
            if (await DoesResourceExist(customerId, "customers", documentUri))
            {
                try
                {
                    var response = client.DeleteDocumentAsync(documentUri);
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