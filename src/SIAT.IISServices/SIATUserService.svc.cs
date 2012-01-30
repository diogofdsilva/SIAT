using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIAT.UserInfo.Contract;
using SIAT.UserInfo.Contract.DTO;
using SIAT.UserInfo.DAL;

namespace SIAT.IISServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SIATUserService" in code, svc and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class SIATUserService : IUserInfo
    {

        public User Login(string email, string password)
        {
            using (UserDataAccessLayer layer = new UserDataAccessLayer())
            {
                User user = layer.Users.Get(email);

                if (user != null && user.Pass == password)
                {
                    return user;
                }

                return null;
            }
        }

        public User GetUserByFacebookId(long facebookId)
        {
            using (UserDataAccessLayer layer = new UserDataAccessLayer())
            {
                return layer.Users.GetByFacebookId(facebookId);
            }
        }

        public User GetUser(int id)
        {
            using (UserDataAccessLayer layer = new UserDataAccessLayer())
            {
                return layer.Users.Get(id);
            }
        }

        public User GetUserByEmail(string email)
        {
            using (UserDataAccessLayer layer = new UserDataAccessLayer())
            {
                return layer.Users.Get(email);
            }
        }

        public List<User> GetAllUserFriends(int id)
        {
            using (UserDataAccessLayer layer = new UserDataAccessLayer())
            {
                User e = layer.Users.Get(id);

                return e.Friends;
            }
        }

        public User NewUser(User e)
        {
            using (UserDataAccessLayer layer = new UserDataAccessLayer())
            {
                e = layer.Users.Add(e);
                layer.Commit();
                return e;
            }
        }
    }
}
