using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIAT.DAL.Shared;
using SIAT.WP7.Shared.DAL.DataMapper;
using SIAT.WP7.Shared.DAL.EFModel;

namespace SIAT.WP7.Shared.DAL
{
    public class WP7InfoDataAccessLayer : IUnitOfWork
    {
        private WP7Entities _entities;

        public WP7InfoDataAccessLayer()
        {
            _entities = new WP7Entities();
        }

        public ISubscriptionsDataMapper Subcriptions
        {
            get
            {
                return _subcriptions ?? (_subcriptions = new SubscriptionsDataMapper(_entities));
            }
        }

        private ISubscriptionsDataMapper _subcriptions;

        #region Implementation of IDisposable

        public void Dispose()
        {
            _entities.Dispose();
        }

        #endregion

        #region Implementation of IUnitOfWork

        public void Commit()
        {
            _entities.SaveChanges();
        }

        public void Rollback()
        {
            
        }

        #endregion
    }
}
