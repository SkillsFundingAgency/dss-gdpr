using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace NCS.DSS.DataUtility.Services
{
    public class IdentifyAndAnonymiseDataService : IIdentifyAndAnonymiseDataService
    {
        private readonly string _GDPRUpdateCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRUpdateCustomersStoredProcedureName");
        private readonly string _GDPRIdentifyCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRIdentifyCustomersStoredProcedureName");
        private readonly string _sqlConnectionString = Environment.GetEnvironmentVariable("AzureSQLConnectionString");

        private readonly ILogger<IIdentifyAndAnonymiseDataService> _logger;
        private readonly ICosmosDBService _cosmosDBService;
        private readonly SqlConnection _sqlConnection;

        public IdentifyAndAnonymiseDataService(ICosmosDBService cosmosDBService, ILogger<IIdentifyAndAnonymiseDataService> logger)
        {
            _cosmosDBService = cosmosDBService;
            _logger = logger;
            _sqlConnection = new SqlConnection(_sqlConnectionString);
        }

        public async Task AnonymiseData()
        {
            await ExecuteUpdateStoredProcedureAsync();
        }

        public async Task DeleteCustomersFromCosmos(List<Guid> CustomerIds)
        {
            if (CustomerIds != null)
            {
                foreach (Guid customerId in CustomerIds)
                {
                    await _cosmosDBService.DeleteRecordsForCustomer(customerId);
                }
            }
        }

        public async Task<List<Guid>> ReturnCustomerIds()
        {
            return await ExecuteIdentifyStoredProcedureAsync();
        }

        private async Task<List<Guid>> ExecuteIdentifyStoredProcedureAsync()
        {
            using var command = new SqlCommand(_GDPRIdentifyCustomersStoredProcedureName, _sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            _logger.LogInformation($"Attempting to execute the stored procedure: {_GDPRIdentifyCustomersStoredProcedureName}");
            _logger.LogInformation("Opening a database connection...");

            try
            {
                await _sqlConnection.OpenAsync();
                SqlDataReader reader = command.ExecuteReader();

                List<Guid> idList = new List<Guid>();
                Guid id;

                while (reader.Read())
                {
                    id = Guid.Parse(reader["ID"].ToString());
                    idList.Add(id);
                }

                _logger.LogInformation($"Finished executing the stored procedure: {_GDPRIdentifyCustomersStoredProcedureName}");
                return idList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to execute the stored procedure ({_GDPRIdentifyCustomersStoredProcedureName}). Error: {ex.Message}");
                throw;
            }
            finally
            {
                _logger.LogInformation("Closing a database connection...");
                await _sqlConnection.CloseAsync();
            }
        }

        private async Task ExecuteUpdateStoredProcedureAsync()
        {
            using var command = new SqlCommand(_GDPRUpdateCustomersStoredProcedureName, _sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            _logger.LogInformation($"Attempting to execute the stored procedure: {_GDPRUpdateCustomersStoredProcedureName}");
            _logger.LogInformation("Opening a database connection...");

            try
            {
                await _sqlConnection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                _logger.LogInformation($"Finished executing the stored procedure: {_GDPRUpdateCustomersStoredProcedureName}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to execute the stored procedure ({_GDPRUpdateCustomersStoredProcedureName}). Error: {ex.Message}");
                throw;
            }
            finally
            {
                _logger.LogInformation("Closing a database connection...");
                await _sqlConnection.CloseAsync();
            }
        }
    }
}
