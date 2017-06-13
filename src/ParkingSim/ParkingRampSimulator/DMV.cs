using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public static class DMV
    {
        public static List<string> Plates = new List<string>();

        public static string GetPlate()
        {
            string result = RandomString(6);
            while (Plates.Contains(result))
                result = RandomString(6);
            Plates.Add(result);
            return result;
        }

        public static string RandomString(int length)
        {
            const string chars = "BCDGHJKLMNPQRSVWXZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Simulator.Random.Next(s.Length)]).ToArray());
        }
    }
}
