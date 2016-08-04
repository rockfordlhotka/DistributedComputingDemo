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
        public bool IsFull { get { return (Occupant != null); } }

        public ParkingLocation(string name)
        {
            Name = name;
            Occupant = null;
        }

        public void ParkAuto(Auto auto)
        {
            Occupant = auto;
            Simulator.Notifier.Notify(new AutoParked { Auto = auto, Location = this });
        }

        public Auto AutoDeparts()
        {
            if (Occupant == null)
                throw new InvalidOperationException();
            var result = Occupant;
            Simulator.Notifier.Notify(new AutoDeparted { Auto = Occupant, Location = this });
            Occupant = null;
            return result;
        }

        public class AutoParked
        {
            public Auto Auto { get; set; }
            public ParkingLocation Location { get; set; }

            public override string ToString()
            {
                return Auto.LicensePlate + " " + Auto.DateToDepart + " " + Location.Name;
            }
        }

        public class AutoDeparted
        {
            public Auto Auto { get; set; }
            public ParkingLocation Location { get; set; }

            public override string ToString()
            {
                return Auto.LicensePlate + " " + Location.Name;
            }
        }
    }
}
