using System.Data;
using System.Threading.Tasks;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Service
{
    public interface IIdentifyAndAnonymiseDataService
    {
        Task AnonymiseData();
        Task IdentifyCustomers();
    }
}
