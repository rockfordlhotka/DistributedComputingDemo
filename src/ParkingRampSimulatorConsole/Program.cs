using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var simulator = new ParkingRampSimulator.Simulator();
            Console.WriteLine("Starting");
            simulator.Run();
            Console.ReadLine();
            Console.WriteLine("Stopping");
            simulator.Stop();
            Console.WriteLine("Stopped");
            Console.ReadLine();
        }
    }
}
