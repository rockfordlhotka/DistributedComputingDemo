using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Clock
    {
        public DateTime Now { get; private set; }
        private TimeSpan _interval;

        public Clock(TimeSpan interval)
        {
            _interval = interval;
            Now = DateTime.Now.Subtract(new TimeSpan(365, 0, 0, 0));
        }

        public void Tick()
        {
            Now = Now.Add(_interval);
        }
    }
}
