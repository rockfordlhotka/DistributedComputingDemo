using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ParkingRampSimulator.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ParkingRampSimulatorServices.Controllers
{
    public class FacilityStatusController : ApiController
    {
        public Contracts.FacilityStatus Get()
        {
            var result = new Contracts.FacilityStatus();
            var storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["StorageConnectionString"]);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("facilitysummary");
            if (table.Exists())
            {
                var readop = TableOperation.Retrieve<ConstructStatusEntity>("1", "");
                var data = table.Execute(readop);
                if (data != null)
                {
                    var obj = (ConstructStatusEntity)data.Result;
                    result.OpenLocations = obj.OpenLocations;
                    result.TotalLocations = obj.TotalLocations;
                    result.PercentageOpen = (double)result.OpenLocations / (double)result.TotalLocations * 100.0;
                }
            }
            return result;
        }
    }
}
