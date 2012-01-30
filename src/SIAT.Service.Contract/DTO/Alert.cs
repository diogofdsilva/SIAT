using System.Runtime.Serialization;

namespace SIAT.Service.Contract.DTO
{
    [DataContract]
    public class Alert
    {
        [DataMember]
        private AlertLevel _level;
        [DataMember]
        private double _lat;
        [DataMember]
        private double _lon;

        public Alert(AlertLevel level, double latitude, double longitude)
        {
            _level = level;
            _lat = latitude;
            _lon = longitude;
        }

        public double Latitude
        {
            get { return _lat; }
            set { _lat = value; }
        }

        public double Longitude
        {
            get { return _lon; }
            set { _lon = value; }
        }

        public AlertLevel Level
        {
            get { return _level; }
            set { _level = value; }
        }
        
    }

    public enum AlertLevel
    {
        RoadBlock,
        HighTraffic,
        MediumTraffic
    }
}


