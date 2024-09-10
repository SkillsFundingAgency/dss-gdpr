namespace NCS.DSS.GDPRUpdateCustomerInformation.DB
{
    public interface IAzureSqlDbProvider
    {
        public Task<List<Guid>> ExecuteStoredProcedureAsync(string storedProcedureName);
    }
}
