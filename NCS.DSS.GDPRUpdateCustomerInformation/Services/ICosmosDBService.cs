namespace NCS.DSS.GDPRUpdateCustomerInformation.Services
{
    public interface ICosmosDBService
    {
        Task DeleteRecordsForCustomer(Guid customerId);
    }
}
