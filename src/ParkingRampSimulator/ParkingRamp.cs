using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingRamp
    {
        public string Name { get; private set; }
        public List<Floor> Floors { get; private set; }

        public ParkingRamp(string name, int floorCount)
        {
            Name = name;
            Floors = new List<Floor>();
            for (int i = 0; i < floorCount; i++)
            {
                Floors.Add(new Floor(string.Format("{0}-{1}", Name, i.ToString()), 100));
            }
        }
    }
}
