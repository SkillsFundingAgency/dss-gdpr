using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Services
{
    public class CosmosDBService : ICosmosDBService
    {
        private readonly CosmosClient _cosmosDbClient;
        private readonly ILogger<CosmosDBService> _logger;

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

        public CosmosDBService(CosmosClient cosmosClient, ILogger<CosmosDBService> logger)
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

            await DeleteDocumentFromContainer(customerId, CustomerCosmosDb, CustomerCosmosDb);

            _logger.LogInformation($"{nameof(DeleteRecordsForCustomer)} function has finished invocation");
        }

        private async Task DeleteDocumentFromContainer(Guid customerId, string databaseName, string containerName)
        {
            _logger.LogInformation($"Attempting to retrieve documents associated with customer '{customerId.ToString()}' from container '{containerName}' from within database '{databaseName}'");

            Container cosmosDbContainer = _cosmosDbClient.GetContainer(databaseName, containerName);

            QueryDefinition queryDefinition = containerName == CustomerCosmosDb ?
                new QueryDefinition("SELECT * FROM c WHERE c.id = @customerId").WithParameter("@customerId", customerId)
                : new QueryDefinition("SELECT * FROM c WHERE c.CustomerId = @customerId").WithParameter("@customerId", customerId);

            FeedIterator<dynamic> resultSet = cosmosDbContainer.GetItemQueryIterator<dynamic>(queryDefinition);

            List<string> documentIds = new List<string>();

            while (resultSet.HasMoreResults)
            {
                FeedResponse<dynamic> documentRetrievalRequest = await resultSet.ReadNextAsync();
                foreach (var document in documentRetrievalRequest)
                {
                    documentIds.Add(Convert.ToString(document.id));
                }
            }

            if (documentIds.Count > 0)
            {
                _logger.LogInformation($"Customer ({customerId.ToString()}) has a total of {documentIds.Count.ToString()} '{containerName}' documents");
                int totalDeleted = 0;

                foreach (var documentId in documentIds)
                {
                    using (ResponseMessage deleteRequestResponse = await cosmosDbContainer.DeleteItemStreamAsync(documentId, new PartitionKey()))
                    {
                        if (!deleteRequestResponse.IsSuccessStatusCode)
                        {
                            _logger.LogWarning($"Failed to delete document. Document ID: {documentId}");
                        }
                        else
                        {
                            totalDeleted++;
                        }
                    }
                }

                _logger.LogInformation($"{totalDeleted.ToString()} / {documentIds.Count.ToString()} '{containerName}' documents have been deleted successfully");
            }
            else
            {
                _logger.LogWarning($"No documents of type '{containerName}' were found for customer '{customerId.ToString()}'");
            }
        }
    }
}
