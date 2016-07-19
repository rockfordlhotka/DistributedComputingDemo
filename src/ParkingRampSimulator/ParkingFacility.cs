using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingFacility : ParkingConstruct
    {
        public List<ParkingRamp> ParkingRamps { get; private set; }

        public ParkingFacility()
        {
            ParkingRamps = new List<ParkingRamp>();
        }

        public bool IsFull
        {
            get
            {
                bool result = true;
                foreach (var item in ParkingRamps)
                    if (!item.IsFull)
                    {
                        result = false;
                        break;
                    }
                return result;
            }
        }

        public override void Tick()
        {
            if (!IsFull)
            {
                var gateCapacity = (int)(Simulator.Interval.TotalSeconds / 30 * 5); // 5 lanes, 30 secs/car
                for (int i = 0; i < gateCapacity; i++)
                {
                    if (InQueue.Count > 0)
                    {
                        var ramp = GetOpenRamp();
                        ramp.AutoEntering(InQueue.Dequeue());
                    }
                }
            }
            foreach (var item in ParkingRamps)
                item.Tick();
        }

        private ParkingRamp GetOpenRamp()
        {
            ParkingRamp ramp = null;
            do
            {
                var rampNumber = Simulator.Random.Next(ParkingRamps.Count - 1);
                ramp = ParkingRamps[rampNumber];
            } while (!ramp.IsFull);
            return ramp;
        }
    }
}
