namespace NCS.DSS.GDPRUtility.Services
{
    public interface ICosmosDBService
    {
        Task DeleteRecordsForCustomer(Guid customerId);
    }
}
