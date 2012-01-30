using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SIAT.DAL.Shared;
using SIAT.WP7.Shared.DAL.DTO;
using SIAT.WP7.Shared.DAL.EFModel;

namespace SIAT.WP7.Shared.DAL.DataMapper
{
    public interface ISubscriptionsDataMapper : IDalMapper<Subscription, List<Subscription>, Subscription>
    {
        List<Subscription> GetAllFromWay(long wayId);
        Subscription GetFirstByUri(string uri);
    }

    internal class SubscriptionsDataMapper : ISubscriptionsDataMapper
    {
        private readonly WP7Entities _wp7Entities;


        public SubscriptionsDataMapper(WP7Entities wp7Entities)
        {
            _wp7Entities = wp7Entities;
        }

        #region Implementation of IDalMapper<Subscription,List<Subscription>,Subscription>

        public Subscription Add(Subscription e)
        {
            _wp7Entities.Subscriptions.AddObject(e);
            return e;
        }

        public Subscription Get(Subscription key)
        {
            return _wp7Entities.Subscriptions.SingleOrDefault(o => o.Uri == key.Uri && o.WayId == key.WayId);
        }

        public List<Subscription> GetAll()
        {
            return _wp7Entities.Subscriptions.ToList();
        }

        public Subscription Update(Subscription e)
        {
            return _wp7Entities.Subscriptions.ApplyCurrentValues(e);
        }

        public Subscription Delete(Subscription key)
        {
            _wp7Entities.Subscriptions.DeleteObject(key);
            return key;
        }

        public List<Subscription> GetAllFromWay(long wayId)
        {
            return _wp7Entities.Subscriptions.Where(w => w.WayId == wayId).ToList();
        }

        public Subscription GetFirstByUri(string uri)
        {
            return _wp7Entities.Subscriptions.Where(p => p.Uri == uri).First();
        }


        #endregion
    }
}
