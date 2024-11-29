namespace NCS.DSS.DataUtility.Services
{
    public interface IGenericDataService
    {
        Task DeleteFromCosmos(string database, string container, string field, List<string> values, bool int_bool, bool sql_bool);
    }
}
