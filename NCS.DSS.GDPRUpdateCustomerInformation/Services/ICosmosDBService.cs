namespace NCS.DSS.DataUtility.Services
{
    public interface ICosmosDBService
    {
        Task DeleteRecordsForCustomer(Guid customerId);
        Task DeleteGenericRecordsFromContainer(string databaseName, string containerName, string field, string value, bool int_bool);
    }
}
