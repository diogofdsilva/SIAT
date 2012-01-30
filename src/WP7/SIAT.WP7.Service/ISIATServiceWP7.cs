using System;
using System.ServiceModel;
using SIAT.Services.Contract;

namespace SIAT.WP7.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IISIATServiceWp7" in both code and config file together.
    [ServiceContract]
    public interface ISIATServiceWP7 : ISIATServices
    {
        [OperationContract]
        void CreateOrUpdateOccurrencesSubscription(Uri uri, long wayId);

        [OperationContract]
        void ChangeOccurrencesSubscriptionUri(Uri oldUri, Uri newUri);

        [OperationContract]
        void DeleteOccurrencesSubscription(Uri uri);

    }
}
