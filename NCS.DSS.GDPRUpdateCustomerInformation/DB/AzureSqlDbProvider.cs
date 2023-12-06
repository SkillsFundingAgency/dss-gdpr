using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.DB
{
    public class AzureSqlDbProvider : IAzureSqlDbProvider
    {
        private readonly string _sqlConnString = Environment.GetEnvironmentVariable("AzureSQLConnectionString");

        private readonly ILogger<IAzureSqlDbProvider> _logger;

        public AzureSqlDbProvider(
            ILogger<IAzureSqlDbProvider> logger)
        {
            _logger = logger;
        }

        public void ExecuteStoredProcedure(string storedProcedureName)
        {
            using var conn = new SqlConnection(_sqlConnString);
            using var command = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                _logger.LogInformation($"Attempting to execute the stored procedure: {storedProcedureName}");
                _logger.LogInformation("Attempting to open a database connection");

                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Failed to execute the stored procedure: {e}");
                throw;
            }
            finally
            {
                _logger.LogInformation("Closing the database connection");
                conn.Close();
            }
        }

    }
}
