namespace NCS.DSS.GDPRUpdateCustomerInformation.Services
{
    public interface IIdentifyAndAnonymiseDataService
    {
        Task AnonymiseData();
        Task DeleteCustomersFromCosmos(List<Guid> customerIdList);
        Task<List<Guid>> ReturnCustomerIds();
    }
}
