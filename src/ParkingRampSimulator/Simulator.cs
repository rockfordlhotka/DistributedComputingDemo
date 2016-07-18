using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Simulator
    {
        public ParkingFacility Facility { get; private set; }

        public Simulator()
        {
            Facility = new ParkingFacility();
            Facility.ParkingRamps.Add(new ParkingRamp("Red", 4));
            Facility.ParkingRamps.Add(new ParkingRamp("Gold", 6));
            Facility.ParkingRamps.Add(new ParkingRamp("Green", 4));
            Facility.ParkingRamps.Add(new ParkingRamp("Blue", 5));
        }
    }
}
