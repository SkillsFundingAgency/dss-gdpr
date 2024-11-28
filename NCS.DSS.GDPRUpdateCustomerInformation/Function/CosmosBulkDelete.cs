using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NCS.DSS.DataUtility.Services;

namespace NCS.DSS.DataUtility.Function
{
    public class CosmosBulkDelete
    {
        private readonly IGenericDataService _genericDataService;
        private readonly ILogger<CosmosBulkDelete> _logger;

        public CosmosBulkDelete(IGenericDataService genericDataService, ILogger<CosmosBulkDelete> logger)
        {
            _genericDataService = genericDataService;
            _logger = logger;
        }

        [Function(nameof(CosmosBulkDelete))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation($"{nameof(CosmosBulkDelete)} has been invoked");
            _logger.LogInformation($"Query recieved: {req.QueryString}");
            _logger.LogInformation("Attempting to retrieve the db-name, container-name, field-name, and field-values of the records to delete");

            try
            {
                string database = req.Query["db-name"];
                string container = req.Query["container-name"];
                string field = req.Query["field-name"];
                List<string> values = (req.Query["field-values"].FirstOrDefault() ?? string.Empty).Split(',').ToList();
                bool sql = bool.TryParse(req.Query["sql-delete"], out sql);

                _logger.LogInformation($" Found paramaters db-name: '{database}', container-name: '{container}', field-name: '{field}', and field-values: '{values.ToString()}'");

                await _genericDataService.DeleteFromCosmos(database, container, field, values, sql);

                _logger.LogInformation($"{nameof(CosmosBulkDelete)} has finished invocation successfully");

                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CosmosBulkDelete)} has failed to invoke. Error: {ex.Message}");
                throw;
            }
        }
    }
}
