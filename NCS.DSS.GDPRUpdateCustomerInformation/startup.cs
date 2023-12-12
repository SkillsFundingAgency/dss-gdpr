using DFC.JSON.Standard;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Provider;
using NCS.DSS.GDPRUpdateCustomerInformation;
using NCS.DSS.GDPRUpdateCustomerInformation.DB;
using NCS.DSS.GDPRUpdateCustomerInformation.Service;
using NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper;

[assembly: FunctionsStartup(typeof(Startup))]
namespace NCS.DSS.GDPRUpdateCustomerInformation
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddSingleton<IResourceHelper, ResourceHelper>();
            builder.Services.AddSingleton<IJsonHelper, JsonHelper>();
            builder.Services.AddSingleton<IDocumentDBProvider, DocumentDBProvider>();
            builder.Services.AddSingleton<IAzureSqlDbProvider, AzureSqlDbProvider>();
            builder.Services.AddSingleton<IIdentifyAndAnonymiseDataService, IdentifyAndAnonymiseDataService>();
        }
    }
}