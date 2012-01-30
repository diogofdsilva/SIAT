using System;
using System.Linq;
using System.ServiceModel;
using SIAT.Services;
using SIAT.WP7.Shared.DAL;
using SIAT.WP7.Shared.DAL.DTO;

namespace SIAT.WP7.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class SIATServiceWP7 : SIATServices, ISIATServiceWP7
    {
        #region Implementation of ISIATServiceWP7

        public void CreateOrUpdateOccurrencesSubscription(Uri uri, long wayId)
        {
            string stringUri = uri.ToString();
            using (var context = new WP7InfoDataAccessLayer())
            {
                context.Subcriptions.Add(new Subscription(uri.ToString(), wayId));
                context.Commit();
            }
        }

        public void ChangeOccurrencesSubscriptionUri(Uri oldUri, Uri newUri)
        {
            using (var context = new WP7InfoDataAccessLayer())
            {
                Subscription subscription = context.Subcriptions.GetFirstByUri(oldUri.ToString());

                subscription.Uri = newUri.ToString();

                context.Subcriptions.Update(subscription);
                context.Commit();
            }
        }

        public void DeleteOccurrencesSubscription(Uri uri)
        {
            using (var context = new WP7InfoDataAccessLayer())
            {
                Subscription subscription = context.Subcriptions.GetFirstByUri(uri.ToString());
                context.Subcriptions.Delete(subscription);
                context.Commit();
            }
        }

        #endregion
    }
}
