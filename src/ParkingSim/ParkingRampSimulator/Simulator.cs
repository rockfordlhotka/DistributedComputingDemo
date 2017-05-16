using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Simulator
    {
        internal static Random Random = new Random();
        public static Clock Clock { get; private set; }
        public static ParkingFacility ParkingFacility { get; private set; }
        public static Notifier Notifier { get; private set; }

        private AutoGenerator Generator = new AutoGenerator();
        private int _frequency = 100; // ms between simulation runs
        public static TimeSpan Interval { get; private set; }

        static Simulator()
        {
            Notifier = new Notifier();
            Interval = new TimeSpan(0, 7, 17);
            Clock = new Clock(Interval);
            ParkingFacility = new ParkingFacility();
        }

        public Simulator()
        {
            ParkingFacility.ParkingRamps.Add(new ParkingRamp(ParkingFacility, "Red", 4, 100));
            ParkingFacility.ParkingRamps.Add(new ParkingRamp(ParkingFacility, "Gold", 6, 75));
            ParkingFacility.ParkingRamps.Add(new ParkingRamp(ParkingFacility, "Green", 4, 150));
            ParkingFacility.ParkingRamps.Add(new ParkingRamp(ParkingFacility, "Blue", 5, 125));
        }

        private Task _task;
        private CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken =  new CancellationToken();

        public void Run()
        {
            _cancellationToken = _cancellationSource.Token;
            _task = Task.Run(async () => 
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    Clock.Tick();
                    Generator.Tick();
                    ParkingFacility.Tick();
                    await Task.Delay(_frequency, _cancellationToken);
                }
            }, _cancellationToken);
        }

        public void Stop()
        {
            _cancellationSource.Cancel();
            while (!_task.IsCanceled)
            { /* wait to complete */ }
        }
    }
}
