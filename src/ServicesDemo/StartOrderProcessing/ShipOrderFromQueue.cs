using System;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace StartOrderProcessing
{
    public static class ShipOrderFromQueue
    {
        [FunctionName("ShipOrderFromQueue")]
        public static void Run([ServiceBusTrigger("shiporder", Connection = "ShipOrderQueue")]string myQueueItem, TraceWriter log)
        {
            log.Info($"Order to ship: { myQueueItem }");
        }
    }
}
