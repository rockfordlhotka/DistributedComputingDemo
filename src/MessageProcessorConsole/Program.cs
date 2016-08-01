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
            var reader = new AzureMessageReader();
            Microsoft.ServiceBus.Messaging.BrokeredMessage message = null;
            do
            {
                message = reader.RecieveNextMessage();
                //Console.WriteLine(message.ToString());
                Console.WriteLine(reader.LastResult.ToString());
            } while (message != null);
            Console.ReadKey();
        }
    }
}
