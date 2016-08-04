using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicSubConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Processing start");
            var reader = new AzureMessageReader();
            Microsoft.ServiceBus.Messaging.BrokeredMessage message = null;
            do
            {
                message = reader.RecieveNextMessage();
                if (message != null)
                    Console.WriteLine(reader.LastResult.GetType().Name + ": " + reader.LastResult.ToString());
            } while (true);
        }
    }
}
