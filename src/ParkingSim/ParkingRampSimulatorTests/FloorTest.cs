using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class FloorTest
    {
        [TestMethod]
        public void NewFloor()
        {
            var floor = new ParkingFloor(null, "A", 10);
            Assert.AreEqual("A", floor.Name);
            Assert.AreEqual(10, floor.ParkingLocations.Count);
            Assert.AreEqual("A-0", floor.ParkingLocations[0].Name);
            Assert.AreEqual("A-5", floor.ParkingLocations[5].Name);
            Assert.IsFalse(floor.IsFull);
        }

        [TestMethod]
        public void FullFloor()
        {
            var floor = new ParkingFloor(null, "A", 5);
            foreach (var item in floor.ParkingLocations)
            {
                item.ParkAuto(new Auto("", 0));
            }
            Assert.IsTrue(floor.IsFull);
        }

        [TestMethod]
        public void FloorDepartAuto()
        {
            var floor = new ParkingFloor(null, "A", 5);
            foreach (var item in floor.ParkingLocations)
            {
                item.ParkAuto(new Auto("", 0));
            }
            var auto = floor.ParkingLocations[3].Occupant;
            floor.AutoExitingFrom(floor.ParkingLocations[3]);
            Assert.IsNotNull(auto);
            Assert.IsNull(floor.ParkingLocations[3].Occupant);
            Assert.IsFalse(floor.IsFull);
            Assert.AreEqual(1, floor.OutQueueLength);
        }


        [TestMethod]
        public void FloorOpenLocation()
        {
            var floor = new ParkingFloor(null, "A", 5);
            foreach (var item in floor.ParkingLocations)
            {
                item.ParkAuto(new Auto("", 0));
            }
            floor.ParkingLocations[3].AutoDeparts();
            var location = floor.FindOpenLocation();
            Assert.IsNotNull(location);
            Assert.AreEqual(location, floor.ParkingLocations[3]);
        }

        [TestMethod]
        public void FloorTick()
        {
            var facility = new ParkingFacility();
            var ramp = new ParkingRamp(facility, "Red", 0, 100);
            var floor = new ParkingFloor(ramp, "A", 5);
            floor.InQueue.Enqueue(new Auto("", .001));
            floor.Tick();
            Assert.AreEqual(1, floor.ParkingLocations.Where(r => r.Occupant != null).Count());

            Simulator.Clock.Tick();
            floor.Tick();
            Assert.AreEqual(0, floor.ParkingLocations.Where(r => r.Occupant != null).Count());
        }
    }
}
