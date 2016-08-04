using System;

namespace ParkingRampSimulator.Messages
{
    public class ClockTickMessage : Message
    {
        public ClockTickMessage(DateTime now)
            : base(now)
        { }

        public override string ToString()
        {
            return string.Format("{0}", TimeStamp);
        }
    }
}