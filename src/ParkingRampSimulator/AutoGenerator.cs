using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class AutoGenerator
    {
        private List<double> _arrivalRate = new List<double>
        {
            5,4,1,1,1,10,20,40,50,30,20,25,20,25,30,30,20,40,40,30,25,15,10,5
        };

        public void Tick()
        {
            var count = (int)(Simulator.Random.NextDouble() * _arrivalRate[Simulator.Clock.Now.Hour]);
            for (int i = 0; i < count; i++)
            {
                var auto = GenerateAuto();
                Simulator.ParkingFacility.AutoEntering(auto);
            }
        }

        public Auto GenerateAuto()
        {
            var licensePlate = DMV.GetPlate();
            var lengthOfTrip = Simulator.Random.NextDouble() * 10.0 + .001;
            return new Auto(licensePlate, lengthOfTrip);
        }
    }
}
