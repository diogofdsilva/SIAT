using System.Data.Objects;
using SIAT.WP7.Shared.DAL.DTO;

namespace SIAT.WP7.Shared.DAL.EFModel
{
    public class WP7Entities : ObjectContext
    {
        private const string ContainerName = "WP7Entities";
       
        public WP7Entities()
            : base("name=WP7Entities", ContainerName)  
        {
            
        }

        public WP7Entities(string connectionString)
            : base(connectionString, ContainerName)
        {
            
        }

        public ObjectSet<Subscription> Subscriptions
        {
            get
            {
                return _subscriptions ?? (_subscriptions = CreateObjectSet<Subscription>());
            }
        }
        private ObjectSet<Subscription> _subscriptions;
    }
}