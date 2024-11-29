using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NCS.DSS.DataUtility.Services;
using Newtonsoft.Json;

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
            _logger.LogInformation("Attempting to retrieve the db-name, container-name, field-name, and field-values of the records to delete");
            _logger.LogInformation("Attempting to retrieve the values of the optional sql-delete and int-values flags");
            _logger.LogInformation("int-values must be true when providing field-values that are numeric");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);

                string database = data["db-name"];
                string container = data["container-name"];
                string field = data["field-name"];
                List<string> values = [.. data["field-values"].Split(',')];
                bool int_bool;
                bool int_text = bool.TryParse(data["int-values"], out int_bool);
                bool sql_bool;
                bool sql_text = bool.TryParse(data["sql-delete"], out sql_bool);

                _logger.LogInformation($"Found parameters...\n" +
                    $"db-name:               {database}\n" +
                    $"container-name:        {container}\n" +
                    $"field-name:            {field}\n" +
                    $"field-values  (count): {values?.Count}\n" +
                    $"int-values (optional): {int_bool}\n" +
                    $"sql-delete (optional): {sql_bool}");

                await _genericDataService.DeleteFromCosmos(database, container, field, values, int_bool, sql_bool);

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
