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
    /// <summary>
    /// List of status values for display on page
    /// </summary>
    public List<OrderStatus> OrderStatuses { get; set; }
    /// <summary>
    /// Order id value to display on page
    /// </summary>
    public string OrderId { get; set; }
    /// <summary>
    /// Result value to display on page
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// Process intial page load
    /// </summary>
    public void OnGet()
    {
      Result = "starting";
      GetStatuses();
    }

    /// <summary>
    /// Process postback from ship order button
    /// </summary>
    /// <param name="OrderId">Order id</param>
    public void OnPostShipOrder(string OrderId)
    {
      if (string.IsNullOrWhiteSpace(OrderId)) return;

      OrderId = OrderId.Trim();

      RecordShipmentRequest(OrderId);

      //PostShipOrderRequestHttp(OrderId);
      try
      {
        PostShipOrderRequest(OrderId);
        Result = "Order shipment requested";
      }
      catch (Exception ex)
      {
        Result = "Order not shipped: " + ex.Message;
      }
      Response.Redirect("/ShipOrder");
    }

    /// <summary>
    /// Record that shipment was requested for this order
    /// </summary>
    /// <param name="id">Order id</param>
    private static void RecordShipmentRequest(string id)
    {
      var client = new HttpClient();
      var json = JsonConvert.SerializeObject(new OrderStatus { OrderId = id, Status = "Requested" });
      var content = new StringContent(json, Encoding.UTF8, "application/json");
      var task = client.PutAsync("http://orderstatusservice/api/orders/" + id, content);
      task.Wait();
      var result = task.Result;
    }

    /// <summary>
    /// Send shipping request for order via synchronous http
    /// </summary>
    /// <param name="id">Order id</param>
    private void PostShipOrderRequestHttp(string id)
    {
      var client = new HttpClient();
      var result = client.GetAsync("http://localhost:7071/api/ProcessOrder?id=" + id).Result;
      if (result.IsSuccessStatusCode)
        Result = "Order shipment requested";
      else
        Result = "Order not shipped: " + result.ReasonPhrase;
    }

    /// <summary>
    /// Send shipping request for order via service bus
    /// </summary>
    /// <param name="id">Order id</param>
    private void PostShipOrderRequest(string id)
    {
      var client = new QueueClient(
        "Endpoint=sb://servicesdemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YGYKoDvilWA0E7is+Vp6fxrulRkIqDKKOw71YC0ePDs=", "shiporder");
      var message = new Message(Encoding.UTF8.GetBytes(id));
      client.SendAsync(message).Wait();
    }

    /// <summary>
    /// Get list of orders being processes
    /// </summary>
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
  }
}