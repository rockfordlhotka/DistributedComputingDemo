using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessor
{
    public class AzureMessageReader
    {
        static string ConnectionString = ParkingRampSimulator.Config.GetQueueConnectionString();
        static string QueueName = "eventstream";

        public object LastResult { get; private set; }

        public BrokeredMessage RecieveNextMessage()
        {
            var client = QueueClient.CreateFromConnectionString(ConnectionString, QueueName);
            var message = client.Receive();
            var raw = message.GetBody<byte[]>();
            LastResult = Deserialize(raw);
            return message;
        }

        private static T Deserialize<T>(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    JsonSerializer ser = new JsonSerializer();
                    return ser.Deserialize<T>(jsonReader);
                }
            }
        }

        private static object Deserialize(Stream s, Type resultType)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    JsonSerializer ser = new JsonSerializer();
                    return ser.Deserialize(jsonReader, resultType);
                }
            }
        }

        private static object Deserialize(byte[] data)
        {
            using (var buffer = new MemoryStream(data))
            {
                buffer.Position = 0;
                using (StreamReader reader = new StreamReader(buffer))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        return ser.Deserialize(jsonReader);
                    }
                }
            }
        }
    }
}
