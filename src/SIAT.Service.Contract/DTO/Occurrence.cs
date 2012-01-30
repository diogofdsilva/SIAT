using System.Runtime.Serialization;

namespace SIAT.Service.Contract.DTO
{
    [DataContract]
    public class Occurrence
    {
        [DataMember]
        private int _id;
        [DataMember]
        private double _lat;
        [DataMember]
        private double _lon;
        [DataMember]
        private byte _intensity;
        [DataMember]
        private byte _precision;
        [DataMember]
        private string _wayName;
        [DataMember]
        private long _wayId;
        [DataMember]
        private string _description;
        [DataMember]
        private long _idCurrentNode;
        [DataMember]
        private long? _idLastNode;



        public Occurrence(double latitude, double longitude, byte intensity, long wayId)
        {
            _lon = longitude;
            _lat = latitude;
            _intensity = intensity;
            _wayId = wayId;
        }

        public Occurrence()
        {
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
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

        public byte Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public byte Intensity
        {
            get { return _intensity; }
            set { _intensity = value; }
        }

        public string WayName
        {
            get { return _wayName; }
            set { _wayName = value; }
        }

        public long WayId
        {
            get { return _wayId; }
            set { _wayId = value; }
        }

        public long IdCurrentNode
        {
            get { return _idCurrentNode; }
            set { _idCurrentNode = value; }
        }

        public long? IdLastNode
        {
            get { return _idLastNode; }
            set { _idLastNode = value; }
        }
    }
}
