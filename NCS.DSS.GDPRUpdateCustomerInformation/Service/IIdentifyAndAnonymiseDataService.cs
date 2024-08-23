namespace NCS.DSS.GDPRUpdateCustomerInformation.Service
{
    public interface IIdentifyAndAnonymiseDataService
    {
        Task AnonymiseData();
        Task DeleteCustomersFromCosmos(List<Guid> customerIdList);
        Task<List<Guid>> ReturnCustomerIds();
    }
}
