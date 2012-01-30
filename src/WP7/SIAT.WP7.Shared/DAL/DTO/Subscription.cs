namespace SIAT.WP7.Shared.DAL.DTO
{
    public class Subscription
    {
        private string _uri;
        private long _wayId;

        public Subscription(string uri, long wayId)
        {
            _uri = uri;
            _wayId = wayId;
        }

        public Subscription()
        {
        }

        public string Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        public long WayId
        {
            get { return _wayId; }
            set { _wayId = value; }
        }
    }
}
