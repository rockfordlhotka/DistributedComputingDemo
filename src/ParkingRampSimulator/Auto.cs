using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Auto
    {
        private const long TicksInADay = 864000000000;
        public string LicensePlate { get; private set; }
        private DateTime _dateEntered;
        public DateTime DateToDepart { get; private set; }

        public Auto(string licensePlate, double lengthOfTrip)
        {
            LicensePlate = licensePlate;
            _dateEntered = Simulator.Clock.Now;
            var span = (long)(lengthOfTrip * TicksInADay);
            DateToDepart = _dateEntered.Add(new TimeSpan(span));
        }
    }
}
