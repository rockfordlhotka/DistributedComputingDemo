using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class ParkingConstruct
    {
        protected Queue<Auto> InQueue = new Queue<Auto>();
        protected Queue<Auto> _outQueue = new Queue<Auto>();

        public void AutoEntering(Auto auto)
        {
            InQueue.Enqueue(auto);
        }

        public void AutoLeaving(Auto auto)
        {
            _outQueue.Enqueue(auto);
        }

        public virtual void Tick()
        {

        }
    }
}
