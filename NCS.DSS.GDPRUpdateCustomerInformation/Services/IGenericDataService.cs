namespace NCS.DSS.DataUtility.Services
{
    public interface IGenericDataService
    {
        Task DeleteFromCosmos(string database, string container, string field, List<string> values, bool sql);
        Task<List<Guid>> ReturnRecordIds();
    }
}
