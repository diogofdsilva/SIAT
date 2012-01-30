using System.Collections.Generic;
using SIAT.DAL.Shared;
using SIAT.UserInfo.Contract.DTO;

namespace SIAT.UserInfo.DAL.IDataMapper
{
    public interface IUserDataMapper : IDalMapper<User, List<User>, long>
    {
        User Get(string email);
        User GetByFacebookId(long facebookId);
    }
}