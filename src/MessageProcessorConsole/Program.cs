using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessor
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
                {
                    //Console.WriteLine(message.ToString());
                    Console.WriteLine(reader.LastResult.ToString());
                }
            } while (message != null);
            Console.WriteLine("Processing complete");
            Console.ReadKey();
        }
    }
}
