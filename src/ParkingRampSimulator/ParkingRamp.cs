using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingRamp : ParkingConstruct
    {
        [Newtonsoft.Json.JsonIgnore]
        public List<ParkingFloor> Floors { get; private set; }
        public override bool IsFull
        {
            get { return OpenLocations < 1; }
        }

        public override int OpenLocations
        {
            get
            {
                return Floors.Sum(r => r.OpenLocations) - InQueue.Count;
            }
        }


        public override int TotalLocations
        {
            get
            {
                return Floors.Sum(r => r.TotalLocations);
            }
        }

        public ParkingRamp(ParkingConstruct parent, string name, int floorCount, int locationCount)
            : base(parent, name)
        {
            Name = name;
            Floors = new List<ParkingFloor>();
            for (int i = 0; i < floorCount; i++)
            {
                Floors.Add(new ParkingFloor(this, string.Format("{0}-{1}", Name, i.ToString()), locationCount));
            }
        }

        public override void Tick()
        {
            var openCount = Floors.Count(r => !r.IsFull);
            if (openCount > 0)
            {
                var gateCapacity = (int)(Simulator.Interval.TotalSeconds / 10.0);
                for (int i = 0; i < gateCapacity; i++)
                {
                    var floorsWithRoom = Floors.Where(r => !r.IsFull).ToList();
                    if (InQueue.Count > 0 && floorsWithRoom.Count > 0)
                    {
                        var floor = Simulator.Random.Next(floorsWithRoom.Count);
                        floorsWithRoom[floor].InQueue.Enqueue(InQueue.Dequeue());
                    }
                }
            }
            foreach (var item in Floors)
                item.Tick();
            base.Tick();
            while (OutQueue.Count > 0)
                Parent.OutQueue.Enqueue(OutQueue.Dequeue());
        }
    }
}
