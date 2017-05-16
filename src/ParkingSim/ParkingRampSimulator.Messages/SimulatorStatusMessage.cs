using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator.Messages
{
    public class SimulatorStatusMessage : Message
    {
        public SimulatorStatusMessage(DateTime now)
            :base(now)
        { }

        public DateTime ClockTime { get; set; }
        public DateTime RealTime { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: Real: {2}, Clock: {1}, Status: {3}", TimeStamp, ClockTime, RealTime, Status);
        }
    }
}
