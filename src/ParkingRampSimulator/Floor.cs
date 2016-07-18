using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Floor
    {
        public string Name { get; private set; }
        public List<ParkingLocation> ParkingLocations { get; private set; }

        public Floor(string name, int locationCount)
        {
            Name = name;
            ParkingLocations = new List<ParkingLocation>();
            for (int i = 0; i < locationCount; i++)
            {
                ParkingLocations.Add(new ParkingLocation(string.Format("{0}-{1}", Name, i.ToString())));
            }
        }
    }
}
