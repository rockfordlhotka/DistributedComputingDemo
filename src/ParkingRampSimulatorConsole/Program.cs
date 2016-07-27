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
            Simulator.Notifier.Subscribe(new MessageHandler<SimulatorStatus>());

            Simulator.Notifier.Subscribe(new AzureMessageHandler<ParkingLocation.AutoParkedMessage>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<ParkingLocation.AutoDepartedMessage>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<ParkingFacility.AutoArrivingAtFacility>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<ParkingFacility.AutoAbandoningFacility>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<ParkingFacility.AutoDepartingFacility>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<Clock.ClockTickMessage>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<ParkingConstruct.ConstructStatusMessage>());
            Simulator.Notifier.Subscribe(new AzureMessageHandler<SimulatorStatus>());

            Simulator.Clock.Now = new DateTime(2015, 7, 14, 5, 0, 0);

            var simulator = new Simulator();
            Simulator.Notifier.Notify(new SimulatorStatus { Status = "Starting" });
            simulator.Run();
            Console.ReadLine();
            Simulator.Notifier.Notify(new SimulatorStatus { Status = "Stopping" });
            simulator.Stop();
            Simulator.Notifier.Notify(new SimulatorStatus { Status = "Stopped" });
            Console.ReadLine();
        }

        public class SimulatorStatus
        {
          public SimulatorStatus()
          {
            RealTime = DateTime.Now;
            SimulatorTime = Simulator.Clock.Now;
          }

          public string Status { get; set; }
          public DateTime RealTime { get; set; }
          public DateTime SimulatorTime { get; set; }

          public override string ToString()
          {
            return string.Format("{0} at {1} ({2})", Status, RealTime, SimulatorTime);
          }
    }
  }
}
