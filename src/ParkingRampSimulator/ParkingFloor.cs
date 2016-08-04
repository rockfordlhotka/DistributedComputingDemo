using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingFloor : ParkingConstruct
    {
        [Newtonsoft.Json.JsonIgnore]
        public List<ParkingLocation> ParkingLocations { get; private set; }
        public override bool IsFull
        {
            get { return OpenLocations < 1; }
        }

        public override int OpenLocations
        {
            get { return ParkingLocations.Count(r => !r.IsFull) - InQueue.Count; }
        }

        public override int TotalLocations
        {
            get { return ParkingLocations.Count; }
        }

        public ParkingFloor(ParkingConstruct parent, string name, int locationCount)
            : base(parent, name)
        {
            ParkingLocations = new List<ParkingLocation>();
            for (int i = 0; i < locationCount; i++)
            {
                ParkingLocations.Add(new ParkingLocation(string.Format("{0}-{1}", Name, i.ToString())));
            }
        }

        public override void Tick()
        {
            var openCount = ParkingLocations.Count(r => !r.IsFull);
            if (openCount > 0)
            {
                while (InQueue.Count > 0)
                {
                    var location = FindOpenLocation();
                    if (location != null)
                        location.ParkAuto(InQueue.Dequeue());
                    else
                        break;
                }
            }

            var departing = ParkingLocations.Where(r => r.Occupant != null && r.Occupant.DateToDepart <= Simulator.Clock.Now);
            foreach (var item in departing)
                AutoExitingFrom(item);
            base.Tick();
            while (OutQueue.Count > 0)
                Parent.OutQueue.Enqueue(OutQueue.Dequeue());
        }

        public void AutoExitingFrom(ParkingLocation location)
        {
            var auto = location.AutoDeparts();
            OutQueue.Enqueue(auto);
        }

        public ParkingLocation FindOpenLocation()
        {
            var open = ParkingLocations.Where(r => r.Occupant == null);
            var count = open.Count();
            if (count > 0)
            {
                var location = Simulator.Random.Next(count - 1);
                return open.ToList()[location];
            }
            else
            {
                return null;
            }
        }
    }
}
