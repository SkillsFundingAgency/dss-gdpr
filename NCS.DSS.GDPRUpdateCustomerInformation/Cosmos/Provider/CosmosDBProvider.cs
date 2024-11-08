using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using NCS.DSS.GDPRUpdateCustomerInformation.Models;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public class CosmosDBProvider : ICosmosDBProvider
    {
        private readonly CosmosClient _cosmosDbClient;

        private const string ActionPlansCosmosDb = "actionplans";
        private const string ActionsCosmosDb = "actions";
        private const string AddressCosmosDb = "addresses";
        private const string ContactCosmosDb = "contacts";
        private const string CustomerCosmosDb = "customers";
        private const string DigitalIdentityCosmosDb = "digitalidentities";
        private const string DiversityDetailsCosmosDb = "diversitydetails";
        private const string EmploymentProgressionCosmosDb = "employmentprogressions";
        private const string GoalsCosmosDb = "goals";
        private const string LearningProgressionCosmosDb = "learningprogressions";
        private const string OutcomesCosmosDb = "outcomes";
        private const string SessionCosmosDb = "sessions";
        private const string SubscriptionsCosmosDb = "subscriptions";
        private const string TransferCosmosDb = "transfers";
        private const string WebchatsCosmosDb = "webchats";

        public CosmosDBProvider(CosmosClient cosmosClient)
        {
            _cosmosDbClient = cosmosClient;
        }

        public async Task DeleteRecordsForCustomer(Guid customerId)
        {
            var actionPlansTask = DeleteDocumentFromContainer(customerId, ActionPlansCosmosDb, ActionPlansCosmosDb);
            var actionsTask = DeleteDocumentFromContainer(customerId, ActionsCosmosDb, ActionsCosmosDb);
            var addressesTask = DeleteDocumentFromContainer(customerId, AddressCosmosDb, AddressCosmosDb);
            var contactsTask = DeleteDocumentFromContainer(customerId, ContactCosmosDb, ContactCosmosDb);
            var employmentProgressionTask = DeleteDocumentFromContainer(customerId, EmploymentProgressionCosmosDb, EmploymentProgressionCosmosDb);
            var goalsTask = DeleteDocumentFromContainer(customerId, GoalsCosmosDb, GoalsCosmosDb);
            var webchatsTask = DeleteDocumentFromContainer(customerId, WebchatsCosmosDb, WebchatsCosmosDb);
            var digitalIdentityTask = DeleteDocumentFromContainer(customerId, DigitalIdentityCosmosDb, DigitalIdentityCosmosDb);
            var diverityDetailsTask = DeleteDocumentFromContainer(customerId, DiversityDetailsCosmosDb, DiversityDetailsCosmosDb);
            var learningProgressionsTask = DeleteDocumentFromContainer(customerId, LearningProgressionCosmosDb, LearningProgressionCosmosDb);
            var outcomesTask = DeleteDocumentFromContainer(customerId, OutcomesCosmosDb, OutcomesCosmosDb);
            var sessionsTask = DeleteDocumentFromContainer(customerId, SessionCosmosDb, SessionCosmosDb);
            var subscriptionsTask = DeleteDocumentFromContainer(customerId, SubscriptionsCosmosDb, SubscriptionsCosmosDb);
            var transfersTask = DeleteDocumentFromContainer(customerId, TransferCosmosDb, TransferCosmosDb);

            await Task.WhenAll(actionPlansTask, actionsTask, addressesTask, contactsTask, employmentProgressionTask,
                goalsTask, webchatsTask, digitalIdentityTask, diverityDetailsTask, learningProgressionsTask,
                outcomesTask, sessionsTask, subscriptionsTask, transfersTask);

            await DeleteDocumentFromContainer(customerId, CustomerCosmosDb, CustomerCosmosDb); // do we need to check if this exists first?
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
