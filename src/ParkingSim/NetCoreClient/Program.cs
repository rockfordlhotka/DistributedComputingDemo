using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCoreClient
{
  class Program
  {
    static void Main(string[] args)
    {
      var status = GetStatus().Result;
      Console.WriteLine(status);
    }

    public static async Task<string> GetStatus()
    {
      var client = new HttpClient(new HttpClientHandler
        { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
      var json = await client.GetStringAsync("http://parkingsim.azurewebsites.net/api/FacilityStatus");
      var obj = JsonConvert.DeserializeObject<FacilityStatus>(json);
      return string.Format("Facility is {0:0.#} available", obj.PercentageOpen);
    }
  }
}
