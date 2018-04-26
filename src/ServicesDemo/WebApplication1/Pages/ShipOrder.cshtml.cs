using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.ServiceBus;
using OrderStatusWebSite;
using Newtonsoft.Json;

namespace WebApplication1.Pages
{
  public class ShipOrderModel : PageModel
  {
    public void OnGet()
    {
      Result = "starting";
      GetStatuses();
    }

    private void GetStatuses()
    {
      try
      {
        var client = new HttpClient();
        var response = client.GetAsync("http://orderstatusservice/api/orders").Result;
        var data = response.Content.ReadAsStringAsync().Result;
        OrderStatuses = JsonConvert.DeserializeObject<List<OrderStatus>>(data);
      }
      finally
      {
        if (OrderStatuses == null)
          OrderStatuses = new List<OrderStatus>();
      }
    }

    public List<OrderStatus> OrderStatuses { get; set; }
    public string OrderId { get; set; }
    public string Result { get; set; }

    public void OnPostShipOrder(string OrderId)
    {
      if (string.IsNullOrWhiteSpace(OrderId)) return;

      OrderId = OrderId.Trim();
      var client = new HttpClient();
      var json = JsonConvert.SerializeObject(new OrderStatus { OrderId = OrderId, Status = "Requested" });
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var task = client.PutAsync("http://orderstatusservice/api/orders/" + OrderId, content);
      task.Wait();
      var result = task.Result;

      //var client = new HttpClient();
      //var result = client.GetAsync("http://localhost:7071/api/ProcessOrder?id=" + OrderId).Result;
      //if (result.IsSuccessStatusCode)
      //  Result = "Order shipment requested";
      //else
      //  Result = "Order not shipped: " + result.ReasonPhrase;

      try
      {
        PostShipOrderRequest(OrderId);
        Result = "Order shipment requested";
      }
      catch (Exception ex)
      {
        Result = "Order not shipped: " + ex.Message;
      }
      GetStatuses();
    }

    private void PostShipOrderRequest(string id)
    {
      var client = new QueueClient("Endpoint=sb://parkingsim.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=cGeFdFfCiTesceUPHAfZ0bprdU0f+th6vqc6/9E+Fvc=", "shiporder");
      var message = new Message(Encoding.UTF8.GetBytes(id));
      client.SendAsync(message).Wait();
    }
  }
}