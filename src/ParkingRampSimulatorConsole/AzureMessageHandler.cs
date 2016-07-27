using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ParkingRampSimulator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulatorConsole
{
    public class AzureMessageHandler<M> : ISubscriber<M>
    {
        static string ConnectionString = @"";
        static string QueueName = "eventstream";

        public void HandleMessage(M message)
        {
            if (!string.IsNullOrWhiteSpace(message.ToString()))
            {
                var client = QueueClient.CreateFromConnectionString(ConnectionString, QueueName);
                using (var buffer = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(buffer))
                    {
                        using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                        {
                            JsonSerializer ser = new JsonSerializer();
                            ser.Serialize(jsonWriter, message);
                            jsonWriter.Flush();
                            buffer.Position = 0;
                            var outMessage = new BrokeredMessage(buffer);
                            client.Send(outMessage);
                        }
                    }
                }
            }
        }

        //private static void Serialize(object value, Stream s)
        //{
        //    using (StreamWriter writer = new StreamWriter(s))
        //        using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
        //        {
        //            JsonSerializer ser = new JsonSerializer();
        //            ser.Serialize(jsonWriter, value);
        //            jsonWriter.Flush();
        //        }
        //}

        //private static T Deserialize<T>(Stream s)
        //{
        //    using (StreamReader reader = new StreamReader(s))
        //        using (JsonTextReader jsonReader = new JsonTextReader(reader))
        //        {
        //            JsonSerializer ser = new JsonSerializer();
        //            return ser.Deserialize<T>(jsonReader);
        //        }
        //}
    }
}
