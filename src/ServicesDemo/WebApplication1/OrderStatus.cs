using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderStatusWebSite
{
    public class OrderStatus
    {
    public string OrderId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
  }
}
