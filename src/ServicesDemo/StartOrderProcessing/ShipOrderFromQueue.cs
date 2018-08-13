using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace StartOrderProcessing
{
  public static class ShipOrderFromQueue
  {
    private static Random rnd = new Random();

    /// <summary>
    /// Ship order based on shipping request from service bus
    /// </summary>
    /// <param name="myQueueItem">Order id</param>
    /// <param name="log">Logging trace writer</param>
    [FunctionName("ShipOrderFromQueue")]
    public static async Task Run(
      [ServiceBusTrigger("shiporder", Connection = "ShipOrderQueue")]
      string myQueueItem, TraceWriter log)
    {
      var orderId = myQueueItem.Trim();
      if (string.IsNullOrWhiteSpace(orderId))
      {
        log.Info("ERROR: Please pass a name on the query string or in the request body");
        return;
      }

      log.Info($"Order to ship: { myQueueItem }");
      await Task.Delay(rnd.Next(4000) + 1000); // insert artificial delay

      var response = await OrderStatus.SetOrderShippedStatus(orderId, "Shipped");
      log.Info($"Response: { response.StatusCode }: { response.ReasonPhrase }");
    }

  }
}
