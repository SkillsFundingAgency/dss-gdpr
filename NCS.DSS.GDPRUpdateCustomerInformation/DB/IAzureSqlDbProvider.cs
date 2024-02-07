using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.DB
{
    public interface IAzureSqlDbProvider
    {
        public Task<List<Guid>> ExecuteStoredProcedureAsync(string storedProcedureName);
    }
}
