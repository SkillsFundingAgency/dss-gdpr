using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NCS.DSS.GDPRUpdateCustomerInformation.Models;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public class CosmosDBProvider : ICosmosDBProvider
    {
        private readonly CosmosClient _cosmosDbClient;

        public CosmosDBProvider(CosmosClient cosmosClient)
        {
            _cosmosDbClient = cosmosClient;
        }

        public async Task DeleteRecordsForCustomer(Guid customerId)
        {
            var actionPlansTask = DeleteDocumentFromContainer(customerId, "actionplans", "actionplans");
            var actionsTask = DeleteDocumentFromContainer(customerId, "actions", "actions");
            var addressesTask = DeleteDocumentFromContainer(customerId, "addresses", "addresses");
            var contactsTask = DeleteDocumentFromContainer(customerId, "contacts", "contacts");
            var employmentProgressionTask = DeleteDocumentFromContainer(customerId, "employmentprogressions", "employmentprogressions");
            var goalsTask = DeleteDocumentFromContainer(customerId, "goals", "goals");
            var webchatsTask = DeleteDocumentFromContainer(customerId, "webchats", "webchats");
            var digitalIdentityTask = DeleteDocumentFromContainer(customerId, "digitalidentities", "digitalidentities");
            var diverityDetailsTask = DeleteDocumentFromContainer(customerId, "diversitydetails", "diversitydetails");
            var learningProgressionsTask = DeleteDocumentFromContainer(customerId, "learningprogressions", "learningprogressions");
            var outcomesTask = DeleteDocumentFromContainer(customerId, "outcomes", "outcomes");
            var sessionsTask = DeleteDocumentFromContainer(customerId, "sessions", "sessions");
            var subscriptionsTask = DeleteDocumentFromContainer(customerId, "subscriptions", "subscriptions");
            var transfersTask = DeleteDocumentFromContainer(customerId, "transfers", "transfers");

            await Task.WhenAll(actionPlansTask, actionsTask, addressesTask, contactsTask, employmentProgressionTask,
                goalsTask, webchatsTask, digitalIdentityTask, diverityDetailsTask, learningProgressionsTask,
                outcomesTask, sessionsTask, subscriptionsTask, transfersTask);

            await DeleteDocumentFromContainer(customerId, "customers", "customers"); // do we need to check if this exists first?
        }

        private async Task DeleteDocumentFromContainer(Guid customerId, string databaseName, string containerName)
        {
            Container cosmosDbContainer = _cosmosDbClient.GetContainer(databaseName, containerName);

            var query = cosmosDbContainer.GetItemLinqQueryable<DocumentId>()
                .Where(x => x.CustomerId == customerId)
                .ToFeedIterator();

            List<DocumentId> documents = new List<DocumentId>();

            while (query.HasMoreResults)
            {
                var results = await query.ReadNextAsync();
                documents.AddRange(results);
            }

            if (documents != null && documents.Count > 0)
            {
                foreach (var document in documents)
                {
                    await cosmosDbContainer.DeleteItemAsync<DocumentId>(document.Id.ToString(), new PartitionKey());
                }
            }
        }
    }
}
