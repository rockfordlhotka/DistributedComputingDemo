using ParkingRampSimulator;
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
            //Simulator.Notifier.Subscribe(new MessageHandler<ParkingLocation.AutoParkedMessage>());
            //Simulator.Notifier.Subscribe(new MessageHandler<ParkingLocation.AutoDepartedMessage>());
            //Simulator.Notifier.Subscribe(new MessageHandler<ParkingFacility.AutoArrivingAtFacility>());
            Simulator.Notifier.Subscribe(new MessageHandler<ParkingFacility.AutoAbandoningFacility>());
            //Simulator.Notifier.Subscribe(new MessageHandler<ParkingFacility.AutoDepartingFacility>());
            //Simulator.Notifier.Subscribe(new MessageHandler<Clock.ClockTickMessage>());
            Simulator.Notifier.Subscribe(new MessageHandler<ParkingConstruct.ConstructStatusMessage>());

            Simulator.Clock.Now = new DateTime(2015, 7, 14, 5, 0, 0);

            var simulator = new Simulator();
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
