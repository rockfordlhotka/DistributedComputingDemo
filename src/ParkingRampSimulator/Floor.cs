using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Floor : ParkingConstruct
    {
        public string Name { get; private set; }
        public List<ParkingLocation> ParkingLocations { get; private set; }
        public bool IsFull
        {
            get
            {
                var result = true;
                foreach (var item in ParkingLocations)
                    if (!item.IsUsed)
                    {
                        result = false;
                        break;
                    }
                return result;
            }
        }

        public Floor(string name, int locationCount)
        {
            Name = name;
            ParkingLocations = new List<ParkingLocation>();
            for (int i = 0; i < locationCount; i++)
            {
                ParkingLocations.Add(new ParkingLocation(string.Format("{0}-{1}", Name, i.ToString())));
            }
        }

        public override void Tick()
        {
            while (InQueue.Count > 0)
            {
                var location = FindOpenLocation();
                if (location != null)
                {
                    location.ParkAuto(InQueue.Dequeue());
                }
            }

            var departing = ParkingLocations.Where(r => r.Occupant != null && r.Occupant.DateToDepart <= Simulator.Clock.Now);
            foreach (var item in departing)
            {
                AutoLeaving(item.Occupant);
                item.AutoDeparts();              
            }
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
