using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Clock
    {
        public DateTime Now { get; set; }
        private TimeSpan _interval;

        public Clock(TimeSpan interval)
        {
            _interval = interval;
        }

        public void Tick()
        {
            Now = Now.Add(_interval);
            Simulator.Notifier.Notify(new ClockTickMessage { ClockTime = Now });
        }

        public class ClockTickMessage
        {
            public DateTime ClockTime { get; set; }

            public override string ToString()
            {
                return ClockTime.ToString();
            }
        }
    }
}
