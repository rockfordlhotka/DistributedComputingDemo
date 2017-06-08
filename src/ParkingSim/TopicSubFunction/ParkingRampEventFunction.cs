using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace TopicSubFunction
{
  public static class ParkingRampEventFunction
  {
    [FunctionName("ParkingRampEventHandler")]
    public static void Run(
      [ServiceBusTrigger("facilityevents", "function1", AccessRights.Manage, 
                         Connection = "QueueConnection")]byte[] mySbMsg, TraceWriter log)
    {
      log.Info($"C# ServiceBus topic trigger function recieved message of {mySbMsg.Length} bytes");
      var result = Deserialize(mySbMsg);
      log.Info($"C# ServiceBus topic trigger function deserialized: {result.GetType().Name}");
      AzureStorageWriter.WriteToStorage((ParkingRampSimulator.Messages.ConstructStatusMessage)result);
      log.Info($"C# ServiceBus topic trigger function processed: {result.ToString()}");
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