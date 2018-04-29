using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StartOrderProcessing
{
  public class OrderStatus
  {
    public string OrderId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// Record that order was shipped
    /// </summary>
    /// <param name="id">Order id</param>
    /// <returns></returns>
    public static async Task<HttpResponseMessage> SetOrderShippedStatus(string id)
    {
      var client = new HttpClient();
      var json = JsonConvert.SerializeObject(new OrderStatus { OrderId = id, Status = "Shipped" });
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var response = await client.PutAsync("http://localhost:32794/api/orders/" + id, content);
      if (response.StatusCode == System.Net.HttpStatusCode.OK)
        response.ReasonPhrase = $"Order { id } shipped";
      return response;
    }
  }
}