using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<List<Guid>> ExecuteStoredProcedureAsync(string storedProcedureName)
        {
            using SqlConnection conn = new SqlConnection(_sqlConnString);
            using var command = new SqlCommand(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            try
            {
                _logger.LogInformation($"Attempting to execute the stored procedure: {storedProcedureName}");
                _logger.LogInformation("Attempting to open a database connection");

                await conn.OpenAsync();
                command.CommandType = CommandType.StoredProcedure;

                if (storedProcedureName == Environment.GetEnvironmentVariable("GDPRIdentifyCustomersStoredProcedureName")) {
                    SqlDataReader reader = command.ExecuteReader();

                    List<Guid> idList = new List<Guid>();
                    Guid id;

                    while (reader.Read())
                    {
                        id = Guid.Parse(reader["ID"].ToString());
                        idList.Add(id);
                    }

                    _logger.LogInformation("finished running the stored proc");
                    
                    return idList;
                }
                else
                {
                    await command.ExecuteNonQueryAsync();
                    return new List<Guid>();
                }

            }
            catch (Exception e)
            {
                _logger.LogInformation($"Failed to execute the stored procedure: {e}");
                throw;
            }
            finally
            {
                _logger.LogInformation("Closing the database connection");
                await conn.CloseAsync();
            }
        }

    }
}
