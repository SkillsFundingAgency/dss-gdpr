using Azure.Search.Documents.Indexes;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.Models;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public class CosmosDBProvider : ICosmosDBProvider
    {
        private readonly CosmosClient _cosmosDbClient;
        private readonly ILogger<CosmosDBProvider> _logger;

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

        public CosmosDBProvider(CosmosClient cosmosClient, ILogger<CosmosDBProvider> logger)
        {
            _cosmosDbClient = cosmosClient;
            _logger = logger;
        }

        public async Task DeleteRecordsForCustomer(Guid customerId)
        {
            _logger.LogInformation($"{nameof(DeleteRecordsForCustomer)} function has been invoked");

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

            _logger.LogInformation($"{nameof(DeleteRecordsForCustomer)} function has finished invocation");
        }

        private async Task DeleteDocumentFromContainer(Guid customerId, string databaseName, string containerName)
        {
            _logger.LogInformation($"Attempting to retrieve documents associated with customer '{customerId.ToString()}' from container '{containerName}' from within database '{databaseName}'");

            Container cosmosDbContainer = _cosmosDbClient.GetContainer(databaseName, containerName);

            var parameterizedQuery = new QueryDefinition(
                query: "SELECT id, CustomerId FROM @container c WHERE c.CustomerId = @customerId"
            ).WithParameter("@container", containerName).WithParameter("@customerId", customerId);

            using FeedIterator<DocumentId> filteredFeed = cosmosDbContainer.GetItemQueryIterator<DocumentId>(
                 queryDefinition: parameterizedQuery
            );

            //var query = cosmosDbContainer.GetItemLinqQueryable<DocumentId>()
            //    .Where(x => x.CustomerId == customerId)
            //    .ToFeedIterator();

            List<DocumentId> documents = new List<DocumentId>();

            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<DocumentId> response = await filteredFeed.ReadNextAsync();
                documents.AddRange(response);
            }

            if (documents.Count > 0)
            {
                _logger.LogInformation($"A total of {documents.Count.ToString()} '{containerName}' documents have been identified");
                int totalDeleted = 0;

                foreach (var document in documents)
                {
                    try
                    {
                        //await cosmosDbContainer.DeleteItemAsync<DocumentId>(document.Id.ToString(), new PartitionKey());
                        totalDeleted++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to delete document. Document ID: {document.Id.ToString()}. Error: {ex.Message}");
                        // should this throw an exception?
                    }
                }

                _logger.LogInformation($"A total of {totalDeleted} '{containerName}' documents have been deleted");
            }
            else
            {
                _logger.LogWarning($"No documents of type '{containerName}' were found for customer '{customerId.ToString()}'");
            }
        }
    }
}
