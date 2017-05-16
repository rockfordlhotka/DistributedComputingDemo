using System;

namespace ParkingRampSimulator.Messages
{
    public class ExceptionMessage : Message
    {
        public ExceptionMessage(DateTime now)
            : base(now)
        {
        }

        public Exception Exception { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: Exception: {1}", TimeStamp, Exception.ToString());
        }

    }
}