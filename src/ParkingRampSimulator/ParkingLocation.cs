using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingLocation
    {
        public string Name { get; private set; }
        public Auto Occupant { get; private set; }

        public ParkingLocation(string name)
        {
            Name = name;
            Occupant = null;
        }
    }
}
