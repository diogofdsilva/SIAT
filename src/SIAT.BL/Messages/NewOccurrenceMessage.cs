
using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;

namespace SIAT.BL.BusMessages
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