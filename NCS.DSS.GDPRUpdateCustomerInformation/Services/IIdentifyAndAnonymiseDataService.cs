namespace NCS.DSS.GDPRUtility.Services
{
    public interface IIdentifyAndAnonymiseDataService
    {
        Task AnonymiseData();
        Task DeleteCustomersFromCosmos(List<Guid> customerIdList);
        Task<List<Guid>> ReturnCustomerIds();
    }
}
