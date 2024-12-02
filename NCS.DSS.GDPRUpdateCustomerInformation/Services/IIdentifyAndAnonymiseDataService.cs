namespace NCS.DSS.DataUtility.Services
{
    public interface IIdentifyAndAnonymiseDataService
    {
        Task AnonymiseData();
        Task DeleteCustomersFromCosmos(List<Guid> customerIdList);
        Task<List<Guid>> ReturnCustomerIds();
    }
}
