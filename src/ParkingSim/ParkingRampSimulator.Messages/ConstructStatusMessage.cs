using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator.Messages
{
    [DataContract]
    public class ConstructStatusMessage : Message
    {
        public ConstructStatusMessage(DateTime timeStamp)
            : base(timeStamp)
        { }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int TotalLocations { get; set; }
        [DataMember]
        public int OpenLocations { get; set; }
        [DataMember]
        public int InQueueLength { get; set; }
        [DataMember]
        public int OutQueueLength { get; set; }

        public override string ToString()
        {
            var name = Name;
            if (string.IsNullOrWhiteSpace(name))
                name = "facility";
            return string.Format("{0}: Name: {1}, {2}/{3}, In: {4}, Out: {5}", 
                TimeStamp, name, OpenLocations, TotalLocations, InQueueLength, OutQueueLength);
        }
    }
}
