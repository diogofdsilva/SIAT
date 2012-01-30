using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SIAT.UserInfo.Contract.DTO
{
    [DataContract]
    public class User
    {
        [DataMember] private long _id;
        [DataMember] private string _email;
        [DataMember] private string _pass;
        [DataMember] private long? _facebookId;
        [DataMember] private string _firstName;
        [DataMember] private string _lastName;
        [DataMember] private List<User> _friends;

        public List<User> Friends
        {
            get { return _friends; }
            set { _friends = value; }
        }

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }

        public long? FacebookId
        {
            get { return _facebookId; }
            set { _facebookId = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
    }
}