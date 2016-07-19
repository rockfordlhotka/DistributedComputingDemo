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
        public bool IsUsed { get { return (Occupant != null); } }

        public ParkingLocation(string name)
        {
            Name = name;
            Occupant = null;
        }

        public void ParkAuto(Auto auto)
        {
            Occupant = auto;
        }

        public Auto AutoDeparts()
        {
            if (Occupant == null)
                throw new InvalidOperationException();
            var result = Occupant;
            Occupant = null;
            return result;
        }
    }
}
