using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public abstract class ParkingConstruct
    {
        protected ParkingConstruct Parent { get; set; }
        public Queue<Auto> InQueue = new Queue<Auto>();
        public Queue<Auto> OutQueue = new Queue<Auto>();

        public string Name { get; protected set; }

        public virtual int OpenLocations
        {
            get { return 0; }
        }

        public ParkingConstruct(ParkingConstruct parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public virtual bool IsFull
        {
            get
            {
                return true;
            }
        }

        public int InQueueLength
        {
            get { return InQueue.Count; }
        }

        public int OutQueueLength
        {
            get { return OutQueue.Count; }
        }

        public virtual void Tick()
        {
            Simulator.Notifier.Notify(new ConstructStatusMessage { Construct = this });
        }

        public class ConstructStatusMessage
        {
            public ParkingConstruct Construct { get; set; }

            public override string ToString()
            {
                //if (Construct.InQueueLength == 0 && 
                //    Construct.OutQueueLength == 0  && 
                //    Construct.OpenLocations > 0 &&
                //    !Construct.IsFull)
                //    return string.Empty;
                if (Construct.Name.Length > 0) return string.Empty;
                return string.Format("{0} - Open: {4}, In: {1}, Out: {2}, Full: {3}", Construct.Name, Construct.InQueueLength, Construct.OutQueueLength, Construct.IsFull, Construct.OpenLocations);
            }
        }
    }
}
