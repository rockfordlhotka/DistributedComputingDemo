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
            10,8,3,3,3,60,220,120,110,60,40,50,40,60,100,110,60,100,80,60,50,30,20,10
        };
        private List<double> _dayOfWeekFactor = new List<double>
        {
            1.5, 1.5, .5, .5, 1, 1.5, .5
        };


        public void Tick()
        {
            var multiplier = 
                _arrivalRate[Simulator.Clock.Now.Hour] *
                _dayOfWeekFactor[(int)Simulator.Clock.Now.DayOfWeek] * 
                Simulator.Interval.TotalHours;
            var count = (int)(Simulator.Random.NextDouble() * multiplier);
            for (int i = 0; i < count; i++)
            {
                var auto = GenerateAuto();
                Simulator.ParkingFacility.AutoArrives(auto);
            }
        }

        public Auto GenerateAuto()
        {
            var licensePlate = DMV.GetPlate();
            var lengthOfTrip = Simulator.Random.NextDouble() * 7.0 + .001;
            return new Auto(licensePlate, lengthOfTrip);
        }
    }
}
