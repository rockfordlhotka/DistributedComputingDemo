using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator.Messages
{
    public class ConstructStatusMessage : Message
    {
        public ConstructStatusMessage(DateTime timeStamp)
            : base(timeStamp)
        { }

        public string Name { get; set; }
        public int TotalLocations { get; set; }
        public int OpenLocations { get; set; }
        public int InQueueLength { get; set; }
        public int OutQueueLength { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: Name: {1}, {2}/{3}, In: {4}, Out: {5}", 
                TimeStamp, Name, OpenLocations, TotalLocations, InQueueLength, OutQueueLength);
        }
    }
}
