using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OrderStatusService.Controllers
{
  [Produces("application/json")]
  [Route("api/Orders")]
  public class OrdersController : Controller
  {
    private static List<OrderStatus> _orders = new List<OrderStatus>();

    // GET: api/Orders
    /// <summary>
    /// Get list of order status information
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<OrderStatus> Get()
    {
      return _orders;
    }

    // GET: api/Orders/5
    /// <summary>
    /// Get order status information for one order
    /// </summary>
    /// <param name="id">Order id</param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "Get")]
    public OrderStatus Get(string id)
    {
      var item = _orders.Where(_ => _.OrderId == id).FirstOrDefault();
      if (item != null)
        return item;
      else
        throw new ArgumentException("Order does not exist");
    }

    // POST: api/Orders
    /// <summary>
    /// Add or update order status information
    /// </summary>
    /// <param name="value">OrderStatus message</param>
    [HttpPost]
    public void Post([FromBody]OrderStatus value)
    {
      Put(value.OrderId, value);
    }

    // PUT: api/Orders/5
    /// <summary>
    /// Add or update order status information
    /// </summary>
    /// <param name="id">Order id</param>
    /// <param name="value">OrderStatus message</param>
    [HttpPut("{id}")]
    public void Put(string id, [FromBody]OrderStatus value)
    {
      var item = _orders.Where(_ => _.OrderId == id).FirstOrDefault();
      if (item == null)
      {
        item = new OrderStatus { OrderId = id, Status = "Unknown", LastUpdated = DateTimeOffset.Now };
        _orders.Add(item);
      }
      item.Status = value.Status;
      item.LastUpdated = DateTimeOffset.Now;
    }

    // DELETE: api/ApiWithActions/5
    /// <summary>
    /// Remove order status information
    /// </summary>
    /// <param name="id">Order id</param>
    [HttpDelete("{id}")]
    public void Delete(string id)
    {
      var item = _orders.Where(_ => _.OrderId == id).FirstOrDefault();
      if (item != null)
        _orders.Remove(item);
    }
  }
}
