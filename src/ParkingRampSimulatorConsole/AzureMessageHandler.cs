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
        static string ConnectionString = Config.GetQueueConnectionString();
        static string QueueName = "eventstream";

        public void HandleMessage(M message)
        {
            if (!string.IsNullOrWhiteSpace(message.ToString()))
            {
                WriteToQueue(message);
                WriteToAllEvents(message);
                var csm = message as ParkingConstruct.ConstructStatusMessage;
                if (csm != null && csm.Construct.Name.Length == 0)
                    WriteToFacilityEvents(message);
            }
        }

        private void WriteToFacilityEvents(M message)
        {
            var client = TopicClient.CreateFromConnectionString(ConnectionString, "facilityevents");
            using (var buffer = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(buffer))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        ser.Serialize(jsonWriter, message);
                        jsonWriter.Flush();
                        buffer.Position = 0;
                        var outMessage = new BrokeredMessage(buffer.ToArray());
                        client.Send(outMessage);
                    }
                }
            }
        }

        private void WriteToAllEvents(M message)
        {
            var client = TopicClient.CreateFromConnectionString(ConnectionString, "allevents");
            using (var buffer = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(buffer))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        ser.Serialize(jsonWriter, message);
                        jsonWriter.Flush();
                        buffer.Position = 0;
                        var outMessage = new BrokeredMessage(buffer.ToArray());
                        client.Send(outMessage);
                    }
                }
            }
        }

        private static void WriteToQueue(M message)
        {
            var client = QueueClient.CreateFromConnectionString(ConnectionString, QueueName);
            using (var buffer = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(buffer))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        ser.Serialize(jsonWriter, message);
                        jsonWriter.Flush();
                        buffer.Position = 0;
                        var outMessage = new BrokeredMessage(buffer.ToArray());
                        client.Send(outMessage);
                    }
                }
            }
        }
    }
}
