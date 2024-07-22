using System;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public interface IDocumentDBProvider
    {
        Task<bool> DoesResourceExist(Guid customerId, string collection, Uri documentUri);
        Task DeleteRecordsForCustomer(Guid customerId);
    }
}