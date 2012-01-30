using System;
using System.Linq;
using System.Collections.Generic;
using SIAT.UserInfo.Contract.DTO;
using SIAT.UserInfo.DAL.EFEntities;
using SIAT.UserInfo.DAL.IDataMapper;

namespace SIAT.UserInfo.DAL.DataMapper
{
    public class UserDataMapper : IUserDataMapper
    {
        private UserEntities _entities;

        public UserDataMapper(UserEntities entities)
        {
            _entities = entities;
        }

        public User Add(User e)
        {
            _entities.Users.AddObject(e);
            return e;
        }

        public User Get(long key)
        {
            return _entities.Users.SingleOrDefault(o => o.Id == key);
        }

        public List<User> GetAll()
        {
            return _entities.Users.ToList();
        }

        public User Update(User e)
        {
            _entities.Users.ApplyCurrentValues(e);
            return e;
        }

        public User Delete(long key)
        {
            User user = Get(key);

            if (user != null)
            {
                _entities.Users.DeleteObject(user);
            }

            return user;
        }

        public User Get(string email)
        {
            return _entities.Users.SingleOrDefault(o => o.Email == email);
        }

        public User GetByFacebookId(long facebookId)
        {
            return _entities.Users.SingleOrDefault(o => o.FacebookId == facebookId);
        }
    }
}