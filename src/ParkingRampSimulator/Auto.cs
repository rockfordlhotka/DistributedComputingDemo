using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Auto
    {
        public string LicensePlate { get; private set; }
        private float _lengthOfStay;

        public Auto(string licensePlate, float lengthOfStay)
        {
            LicensePlate = licensePlate;
            _lengthOfStay = lengthOfStay;
        }
    }
}
