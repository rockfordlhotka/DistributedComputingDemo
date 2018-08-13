using System;
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
    private static Random rnd = new Random();

    [FunctionName("ProcessOrder")]
    public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
    {
      log.Info("C# HTTP trigger function processed a request.");

      // parse query parameter
      string orderId = req.GetQueryNameValuePairs()
          .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
          .Value.Trim();

      if (string.IsNullOrWhiteSpace(orderId))
      {
        log.Info("ERROR: Please pass a name on the query string or in the request body");
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
      }

      log.Info($"Order to ship: { orderId }");
      await Task.Delay(rnd.Next(4000) + 1000); // insert artificial delay

      var response = await OrderStatus.SetOrderShippedStatus(orderId, "Shipped");
      log.Info($"Response: { response.StatusCode }: { response.ReasonPhrase }");
      return response;
    }
  }
}
