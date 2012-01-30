using System.Collections.Generic;
using System.ServiceModel;
using SIAT.UserInfo.Contract.DTO;

namespace SIAT.UserInfo.Contract
{
    [ServiceContract]
    public interface IUserInfo
    {
        [OperationContract]
        User Login(string username, string password);
        [OperationContract]
        User GetUserByFacebookId(long facebookId);
        [OperationContract]
        User GetUser(int id);
        [OperationContract]
        User GetUserByEmail(string email);
        [OperationContract]
        List<User> GetAllUserFriends(int id);
        [OperationContract]
        User NewUser(User e);
    }
}
