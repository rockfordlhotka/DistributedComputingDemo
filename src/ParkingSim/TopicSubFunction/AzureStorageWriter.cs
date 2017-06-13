using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ParkingRampSimulator.Entities;
using ParkingRampSimulator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicSubFunction
{
  public class AzureStorageWriter
  {
    static string storageConnectionString = @"";

    public static void WriteToStorage(ConstructStatusMessage message)
    {
      var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
      var tableClient = storageAccount.CreateCloudTableClient();
      var table = tableClient.GetTableReference("facilitysummary");
      table.CreateIfNotExists();

      var entity = new ConstructStatusEntity(message.Name)
      {
        TimeStamp = message.TimeStamp,
        TotalLocations = message.TotalLocations,
        OpenLocations = message.OpenLocations,
        InQueueLength = message.InQueueLength,
        OutQueueLength = message.OutQueueLength
      };

      var operation = TableOperation.InsertOrReplace(entity);
      table.Execute(operation);
    }
  }
}
