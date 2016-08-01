using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Config
    {
        public static string GetQueueConnectionString()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var file = File.OpenText(path + @"\parkingsim.config"))
            {
                return file.ReadLine();
            }
        }
    }
}
