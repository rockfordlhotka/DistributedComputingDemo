using ParkingRampSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulatorConsole
{
    public class MessageHandler<M> : ISubscriber<M>
    {
        public void HandleMessage(M message)
        {
            var text = message.ToString();
            if (!string.IsNullOrWhiteSpace(text))
                Console.WriteLine(typeof(M).Name + ": " + text);
        }
    }
}
