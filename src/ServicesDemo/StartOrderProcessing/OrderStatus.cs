using System;

namespace StartOrderProcessing
{
  public class OrderStatus
  {
    public string OrderId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
  }
}