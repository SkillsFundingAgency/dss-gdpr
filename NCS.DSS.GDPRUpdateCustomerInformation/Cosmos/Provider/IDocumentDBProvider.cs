using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider
{
    public interface IDocumentDBProvider
    {
        Task<bool> DoesCustomerResourceExist(Guid customerId);
        string GetCustomerJson();
        Task<Models.Customer> GetCustomerByIdAsync(Guid customerId);
    }
}