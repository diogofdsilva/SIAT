using System.Runtime.Serialization;

namespace SIAT.Operations.Messages
{
    [DataContract]
    public class NewOccurrenceMessage
    {
        [DataMember]
        public long WayId{ get; set; }

        [DataMember]
        public string WayName { get; set; }
    }
}