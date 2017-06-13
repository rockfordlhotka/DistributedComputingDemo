using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace ParkingRampSimulator.Entities
{
    public class ConstructStatusEntity : TableEntity
    {
        public ConstructStatusEntity()
        { }

        public ConstructStatusEntity(string name)
        {
            PartitionKey = "1";
            RowKey = name;
        }

        public DateTime TimeStamp { get; set; }
        public int TotalLocations { get; set; }
        public int OpenLocations { get; set; }
        public int InQueueLength { get; set; }
        public int OutQueueLength { get; set; }
    }
}
