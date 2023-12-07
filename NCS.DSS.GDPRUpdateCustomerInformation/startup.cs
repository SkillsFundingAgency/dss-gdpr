using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation;
using NCS.DSS.GDPRUpdateCustomerInformation.DB;
using NCS.DSS.GDPRUpdateCustomerInformation.Service;

[assembly: FunctionsStartup(typeof(Startup))]
namespace NCS.DSS.GDPRUpdateCustomerInformation
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddSingleton<IAzureSqlDbProvider, AzureSqlDbProvider>();
            builder.Services.AddSingleton<IIdentifyAndAnonymiseDataService, IdentifyAndAnonymiseDataService>();
        }
    }
}