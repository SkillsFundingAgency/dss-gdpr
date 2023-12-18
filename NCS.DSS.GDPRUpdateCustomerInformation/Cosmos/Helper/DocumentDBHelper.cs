using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;

namespace NCS.DSS.GDPRUpdateCustomerInformation.Cosmos.Helper
{
    public static class DocumentDBHelper
    {
        private static Uri _actionPlansCollectionUri;
        private static Uri _actionsCollectionUri;
        private static Uri _addressesCollectionUri;
        private static Uri _contactsCollectionUri;
        private static Uri _customersCollectionUri;
        private static Uri _employmentProgressionsCollectionUri;
        private static Uri _goalsCollectionUri;
        private static Uri _webchatsCollectionUri;

        private static Uri _digitalIdentitiesCollectionUri;
        private static Uri _diversityDetailsCollectionUri;
        private static Uri _learningProgressionsCollectionUri;
        private static Uri _outcomesCollectionUri;
        private static Uri _sessionsCollectionUri;
        private static Uri _subscriptionsCollectionUri;
        private static Uri _transfersCollectionUri;

        public static Uri CreateDocumentCollectionUri(string collection)
        {
            switch (collection)
            {
                case "customers":
                    if (_customersCollectionUri == null)
                    {
                        _customersCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _customersCollectionUri;
                case "actionplans":
                    if (_actionPlansCollectionUri == null)
                    {
                        _actionPlansCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _actionPlansCollectionUri;
                case "actions":
                    if (_actionsCollectionUri == null)
                    {
                        _actionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _actionsCollectionUri;
                case "addresses":
                    if (_addressesCollectionUri == null)
                    {
                        _addressesCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _addressesCollectionUri;
                case "contacts":
                    if (_contactsCollectionUri == null)
                    {
                        _contactsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _contactsCollectionUri;
                case "employmentprogressions":
                    if (_employmentProgressionsCollectionUri == null)
                    {
                        _employmentProgressionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _employmentProgressionsCollectionUri;
                case "goals":
                    if (_goalsCollectionUri == null)
                    {
                        _goalsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _goalsCollectionUri;
                case "webchats":
                    if (_webchatsCollectionUri == null)
                    {
                        _webchatsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _webchatsCollectionUri;
                case "digitalidentities":
                    if (_digitalIdentitiesCollectionUri == null)
                    {
                        _digitalIdentitiesCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _digitalIdentitiesCollectionUri;
                case "diversitydetails":
                    if (_diversityDetailsCollectionUri == null)
                    {
                        _diversityDetailsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _diversityDetailsCollectionUri;
                case "learningprogressions":
                    if (_learningProgressionsCollectionUri == null)
                    {
                        _learningProgressionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _learningProgressionsCollectionUri;
                case "outcomes":
                    if (_outcomesCollectionUri == null)
                    {
                        _outcomesCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _outcomesCollectionUri;
                case "sessions":
                    if (_sessionsCollectionUri == null)
                    {
                        _sessionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _sessionsCollectionUri;
                case "subscriptions":
                    if (_subscriptionsCollectionUri == null)
                    {
                        _subscriptionsCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _subscriptionsCollectionUri;
                case "transfers":
                    if (_transfersCollectionUri == null)
                    {
                        _transfersCollectionUri = UriFactory.CreateDocumentCollectionUri(
                            collection,
                            collection);
                    }
                    return _transfersCollectionUri;
                default:
                    return null;
            }
            
        }

        public static Uri CreateDocumentUri(Guid id, string collection)
        {
            return UriFactory.CreateDocumentUri(collection,
                        collection,
                        id.ToString());
        }
    }
}