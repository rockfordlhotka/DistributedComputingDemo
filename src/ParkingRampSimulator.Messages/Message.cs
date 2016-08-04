using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator.Messages
{
    public class Message
    {
        public DateTime TimeStamp { get; set; }

        public Message(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }
    }
}
