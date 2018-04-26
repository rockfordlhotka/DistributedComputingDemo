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

    [FunctionName("ShipOrderFromQueue")]
    public static async Task Run([ServiceBusTrigger("shiporder", Connection = "ShipOrderQueue")]string myQueueItem, TraceWriter log)
    {
      log.Info($"Order to ship: { myQueueItem }");

      await Task.Delay(rnd.Next(2000)); // insert artificial delay

      var orderId = myQueueItem.Trim();
      var client = new HttpClient();
      var json = JsonConvert.SerializeObject(new OrderStatus { OrderId = orderId, Status = "Shipped" });
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await client.PutAsync("http://localhost:32788/api/orders/" + orderId, content);
      log.Info($"Response: { response.StatusCode }: { response.ReasonPhrase }");
    }
  }
}
