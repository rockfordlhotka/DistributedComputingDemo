using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingFacility
    {
        public List<ParkingRamp> ParkingRamps { get; private set; }

        public ParkingFacility()
        {
            ParkingRamps = new List<ParkingRamp>();
        }
    }
}
