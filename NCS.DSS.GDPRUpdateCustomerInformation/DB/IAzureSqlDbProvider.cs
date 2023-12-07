using System.Data;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.DB
{
    public interface IAzureSqlDbProvider
    {
        public Task ExecuteStoredProcedureAsync(string storedProcedureName);
    }
}
