namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public interface ICosmosDBProvider
    {
        Task DeleteRecordsForCustomer(Guid customerId);
    }
}
