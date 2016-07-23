using ParkingRampSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulatorTests
{
    public class MessageHandler<M> : ISubscriber<M>
    {
        public int MessageCount { get; set; }
        public M Message { get; set; }

        public void HandleMessage(M message)
        {
            MessageCount++;
            Message = message;
        }
    }
}
