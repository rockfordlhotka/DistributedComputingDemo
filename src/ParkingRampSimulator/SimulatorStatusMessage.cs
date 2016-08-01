using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class SimulatorStatus
    {
        public SimulatorStatus()
        {
            RealTime = DateTime.Now;
            SimulatorTime = Simulator.Clock.Now;
        }

        public string Status { get; set; }
        public DateTime RealTime { get; set; }
        public DateTime SimulatorTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0} at {1} ({2})", Status, RealTime, SimulatorTime);
        }
    }
}
