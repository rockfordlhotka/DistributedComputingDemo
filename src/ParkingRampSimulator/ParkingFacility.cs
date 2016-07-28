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
            : base(null, string.Empty)
        {
            ParkingRamps = new List<ParkingRamp>();
        }

        public override bool IsFull
        {
            get
            {
                return ParkingRamps.Sum(r => r.OpenLocations) < 1;
            }
        }

        public override int OpenLocations
        {
            get
            {
                return ParkingRamps.Sum(r => r.OpenLocations);
            }
        }

        public override int TotalLocations
        {
            get
            {
                return ParkingRamps.Sum(r => r.TotalLocations);
            }
        }

        public void AutoArrives(Auto auto)
        {
            Simulator.Notifier.Notify(new AutoArrivingAtFacility { Auto = auto });
            if (InQueue.Count < 50)
                InQueue.Enqueue(auto);
            else
                Simulator.Notifier.Notify(new AutoAbandoningFacility { Auto = auto });
        }

        public override void Tick()
        {
            if (!IsFull)
            {
                var gateCapacity = (int)(Simulator.Interval.TotalSeconds / 60.0 * 5);
                for (int i = 0; i < gateCapacity; i++)
                {
                    if (InQueue.Count > 0 && !IsFull)
                    {
                        var ramp = GetOpenRamp();
                        if (ramp != null)
                            ramp.InQueue.Enqueue(InQueue.Dequeue());
                        else
                            break;
                    }
                }
            }
            foreach (var item in ParkingRamps)
                item.Tick();
            base.Tick();
            while (OutQueue.Count > 0)
            {
                var auto = OutQueue.Dequeue();
                Simulator.Notifier.Notify(new AutoDepartingFacility { Auto = auto });
            }
        }

        public ParkingRamp GetOpenRamp()
        {
            ParkingRamp ramp = null;
            if (!IsFull)
            {
                var potentialRamps = ParkingRamps.Where(r => !r.IsFull).ToList();
                if (potentialRamps.Count > 0)
                {
                    var rampNumber = Simulator.Random.Next(potentialRamps.Count);
                    ramp = potentialRamps[rampNumber];
                }
            }
            return ramp;
        }

        public class AutoArrivingAtFacility
        {
            public Auto Auto { get; set; }

            public override string ToString()
            {
                return Auto.LicensePlate + " at " + Simulator.Clock.Now;
            }
        }

        public class AutoAbandoningFacility
        {
            public Auto Auto { get; set; }

            public override string ToString()
            {
                return Auto.LicensePlate + " at " + Simulator.Clock.Now;
            }
        }

        public class AutoDepartingFacility
        {
            public Auto Auto { get; set; }

            public override string ToString()
            {
                return Auto.LicensePlate + " at " + Simulator.Clock.Now;
            }
        }
    }
}
