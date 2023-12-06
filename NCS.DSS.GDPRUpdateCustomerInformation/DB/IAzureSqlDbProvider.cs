using System.Data;

namespace NCS.DSS.GDPRUpdateCustomerInformation.DB
{
    public interface IAzureSqlDbProvider
    {
        public void ExecuteStoredProcedure(string storedProcedureName);
    }
}
