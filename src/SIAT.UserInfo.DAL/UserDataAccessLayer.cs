using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIAT.DAL.Shared;
using SIAT.UserInfo.DAL.DataMapper;
using SIAT.UserInfo.DAL.EFEntities;
using SIAT.UserInfo.DAL.IDataMapper;

namespace SIAT.UserInfo.DAL
{
    public class UserDataAccessLayer : IUnitOfWork
    {
        private UserEntities _entities;

        public UserDataAccessLayer()
        {
            _entities = new UserEntities();
        }

        private IUserDataMapper _users;

        public IUserDataMapper Users
        {
            get { return _users ?? (_users = new UserDataMapper(_entities)); }
        }

        public void Dispose()
        {
            _entities.Dispose();
        }

        public void Commit()
        {
            _entities.SaveChanges();
        }

        public void Rollback()
        {
            //NOTHING
        }
    }
}
