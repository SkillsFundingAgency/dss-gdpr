using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper
{
    public static class DocumentDBHelper
    {
        private static Uri _actionPlansCollectionUri;
        private static readonly string ActionPlansDatabaseId = Environment.GetEnvironmentVariable("ActionPlansDatabaseId");
        private static readonly string ActionPlansCollectionId = Environment.GetEnvironmentVariable("ActionPlansCollectionId");

        private static Uri _customersCollectionUri;
        private static readonly string CustomersDatabaseId = Environment.GetEnvironmentVariable("CustomersDatabaseId");
        private static readonly string CustomersCollectionId = Environment.GetEnvironmentVariable("CustomersCollectionId");

        private static Uri _actionsCollectionUri;
        private static readonly string ActionsDatabaseId = Environment.GetEnvironmentVariable("ActionsDatabaseId");
        private static readonly string ActionsCollectionId = Environment.GetEnvironmentVariable("ActionsCollectionId");

        private static Uri _addressesCollectionUri;
        private static readonly string AddressesDatabaseId = Environment.GetEnvironmentVariable("AddressesDatabaseId");
        private static readonly string AddressesCollectionId = Environment.GetEnvironmentVariable("AddressesCollectionId");

        private static Uri _contactsCollectionUri;
        private static readonly string ContactsDatabaseId = Environment.GetEnvironmentVariable("ContactsDatabaseId");
        private static readonly string ContactsCollectionId = Environment.GetEnvironmentVariable("ContactsCollectionId");

        private static Uri _employmentProgressionsCollectionUri;
        private static readonly string EmploymentProgressionsDatabaseId = Environment.GetEnvironmentVariable("EmploymentProgressionsDatabaseId");
        private static readonly string EmploymentProgressionsCollectionId = Environment.GetEnvironmentVariable("EmploymentProgressionsCollectionId");
        
        private static Uri _goalsCollectionUri;
        private static readonly string GoalsDatabaseId = Environment.GetEnvironmentVariable("GoalsDatabaseId");
        private static readonly string GoalsCollectionId = Environment.GetEnvironmentVariable("GoalsCollectionId");
        
        private static Uri _webchatsCollectionUri;
        private static readonly string WebchatsDatabaseId = Environment.GetEnvironmentVariable("webchatsDatabaseId");
        private static readonly string WebchatsCollectionId = Environment.GetEnvironmentVariable("webchatsCollectionId");

        public static Uri CreateDocumentCollectionUri(string collection)
        {
            switch (collection)
            {
                case "customers":
                    if (_customersCollectionUri == null)
                    {
                        _customersCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            CustomersDatabaseId,
                            CustomersCollectionId);
                    }
                    return _customersCollectionUri;
                case "actionplans":
                    if (_actionPlansCollectionUri == null)
                    {
                        _actionPlansCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            ActionPlansDatabaseId,
                            ActionPlansCollectionId);
                    }
                    return _actionPlansCollectionUri;
                case "actions":
                    if (_actionsCollectionUri == null)
                    {
                        _actionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            ActionsDatabaseId,
                            ActionsCollectionId);
                    }
                    return _actionsCollectionUri;
                case "addresses":
                    if (_addressesCollectionUri == null)
                    {
                        _addressesCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            AddressesDatabaseId,
                            AddressesCollectionId);
                    }
                    return _addressesCollectionUri;
                case "contacts":
                    if (_contactsCollectionUri == null)
                    {
                        _contactsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            ContactsDatabaseId,
                            ContactsCollectionId);
                    }
                    return _contactsCollectionUri;
                case "employmentprogressions":
                    if (_employmentProgressionsCollectionUri == null)
                    {
                        _employmentProgressionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            EmploymentProgressionsDatabaseId,
                            EmploymentProgressionsCollectionId);
                    }
                    return _employmentProgressionsCollectionUri;
                case "goals":
                    if (_goalsCollectionUri == null)
                    {
                        _goalsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            GoalsDatabaseId,
                            GoalsCollectionId);
                    }
                    return _goalsCollectionUri;
                case "webchats":
                    if (_webchatsCollectionUri == null)
                    {
                        _webchatsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            WebchatsDatabaseId,
                            WebchatsCollectionId);
                    }
                    return _webchatsCollectionUri;
                default:
                    return null;
            }
            
        }

        public static Uri CreateDocumentUri(Guid id, string collection)
        {
            switch (collection)
            {
                case "customers":
                    return UriFactory.CreateDocumentUri(CustomersDatabaseId, 
                        CustomersCollectionId, 
                        id.ToString());
                case "actionplans":
                    return UriFactory.CreateDocumentUri(ActionPlansDatabaseId, 
                        ActionPlansCollectionId, 
                        id.ToString());
                case "actions":
                    return UriFactory.CreateDocumentUri(ActionsDatabaseId,
                        ActionsCollectionId,
                        id.ToString());
                case "addresses":
                    return UriFactory.CreateDocumentUri(AddressesDatabaseId,
                        AddressesCollectionId,
                        id.ToString());
                case "contacts":
                    return UriFactory.CreateDocumentUri(ContactsDatabaseId,
                        ContactsCollectionId,
                        id.ToString());
                case "employmentprogressions":
                    return UriFactory.CreateDocumentUri(EmploymentProgressionsDatabaseId,
                        EmploymentProgressionsCollectionId,
                        id.ToString());
                case "goals":
                    return UriFactory.CreateDocumentUri(GoalsDatabaseId,
                        GoalsCollectionId,
                        id.ToString());
                case "webchats":
                    return UriFactory.CreateDocumentUri(WebchatsDatabaseId,
                        WebchatsCollectionId,
                        id.ToString());
                default:
                    return null;

            }
        }
    }
}