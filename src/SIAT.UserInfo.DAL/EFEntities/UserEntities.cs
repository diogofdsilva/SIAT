using System.Data.Objects;
using SIAT.UserInfo.Contract.DTO;

namespace SIAT.UserInfo.DAL.EFEntities
{
    public class UserEntities : ObjectContext
    {
        private const string ContainerName = "UserModelContainer";
       
        public UserEntities()
            : base("name=UserModelContainer", ContainerName)  
        {
            
        }

        public UserEntities(string connectionString)
            : base(connectionString, ContainerName)
        {
            
        }

        public ObjectSet<User> Users
        {
            get
            {
                return _users ?? (_users = CreateObjectSet<User>());
            }
        }
        private ObjectSet<User> _users;
    }
}