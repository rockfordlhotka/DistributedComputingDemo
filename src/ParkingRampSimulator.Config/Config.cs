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
        static string queueConnectionString;
        static string storageConnectionString;

        public static string GetQueueConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(queueConnectionString))
                return queueConnectionString;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var file = File.OpenText(path + @"\parkingsim.config"))
            {
                queueConnectionString = file.ReadLine();
                return queueConnectionString;
            }
        }

        public static string GetStorageConnectionString()
        {
            if (!string.IsNullOrWhiteSpace(storageConnectionString))
                return storageConnectionString;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var file = File.OpenText(path + @"\parkingsim.config"))
            {
                file.ReadLine();
                storageConnectionString = file.ReadLine();
                return storageConnectionString;
            }
        }
    }
}
