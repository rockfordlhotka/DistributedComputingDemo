using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace StartOrderProcessing
{
  public static class ProcessOrder
  {
    [FunctionName("ProcessOrder")]
    public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
    {
      log.Info("C# HTTP trigger function processed a request.");

      // parse query parameter
      string orderId = req.GetQueryNameValuePairs()
          .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
          .Value;
      if (!string.IsNullOrWhiteSpace(orderId))
      {
        var client = new HttpClient();
        var json = JsonConvert.SerializeObject(new OrderStatus { OrderId = orderId, Status = "Shipped" });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PutAsync("http://localhost:32788/api/orders/" + orderId, content);
      }

      return string.IsNullOrWhiteSpace(orderId)
          ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
          : req.CreateResponse(HttpStatusCode.OK, $"Order { orderId } shipped");
    }
  }
}
