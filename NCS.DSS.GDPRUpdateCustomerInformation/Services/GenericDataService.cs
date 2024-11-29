using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace NCS.DSS.DataUtility.Services
{
    public class GenericDataService : IGenericDataService
    {
        private readonly string _GDPRUpdateCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRUpdateCustomersStoredProcedureName");
        private readonly string _GDPRIdentifyCustomersStoredProcedureName = Environment.GetEnvironmentVariable("GDPRIdentifyCustomersStoredProcedureName");
        private readonly string _sqlConnectionString = Environment.GetEnvironmentVariable("AzureSQLConnectionString");

        private readonly ILogger<IGenericDataService> _logger;
        private readonly ICosmosDBService _cosmosDBService;
        private readonly SqlConnection _sqlConnection;

        public GenericDataService(ICosmosDBService cosmosDBService, ILogger<IGenericDataService> logger)
        {
            _cosmosDBService = cosmosDBService;
            _logger = logger;
            _sqlConnection = new SqlConnection(_sqlConnectionString);
        }

        public async Task DeleteFromCosmos(string database, string container, string field, List<string> values, bool sql)
        {
            if (values != null)
            {
                int next = 1;
                foreach (string value in values)
                {
                    _logger.LogInformation($"Looking at value number {next} of {values?.Count}");

                    _logger.LogInformation($"About to initiate Cosmos delete on record(s) with '{field}' value: {value}");
                    await _cosmosDBService.DeleteGenericRecordsFromContainer(database, container, field, value);

                    if (sql)
                    {
                        _logger.LogInformation($"About to initiate SQL delete on record(s) with: '{field}' value: {value}");
                        throw new NotImplementedException();
                    }
                    next++;
                }
            }
        }
    }
}
