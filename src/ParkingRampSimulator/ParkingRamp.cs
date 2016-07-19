using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingRamp : ParkingConstruct
    {
        public string Name { get; private set; }
        public List<Floor> Floors { get; private set; }
        public bool IsFull
        {
            get
            {
                var result = true;
                foreach (var item in Floors)
                    if (!item.IsFull)
                    {
                        result = false;
                        break;
                    }
                return result;
            }
        }

        public ParkingRamp(string name, int floorCount)
        {
            Name = name;
            Floors = new List<Floor>();
            for (int i = 0; i < floorCount; i++)
            {
                Floors.Add(new Floor(string.Format("{0}-{1}", Name, i.ToString()), 100));
            }
        }

        public override void Tick()
        {
            var gateCapacity = (int)(Simulator.Interval.TotalSeconds / 10); // 10 secs/car
            for (int i = 0; i < gateCapacity; i++)
            {
                if (InQueue.Count > 0)
                {
                    var floorsWithRoom = Floors.Where(r => !r.IsFull).ToList();
                    if (floorsWithRoom.Count > 0)
                    {
                        var floor = Simulator.Random.Next(floorsWithRoom.Count - 1);
                        floorsWithRoom[floor].AutoEntering(InQueue.Dequeue());
                    }
                }
            }
            foreach (var item in Floors)
                item.Tick();
        }
    }
}
