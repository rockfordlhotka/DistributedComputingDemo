using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator.Messages
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public DateTime TimeStamp { get; set; }

        public Message(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }
    }
}
