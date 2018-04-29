using System;

namespace OrderStatusService
{
  public class OrderStatus
  {
    public string OrderId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
  }
}