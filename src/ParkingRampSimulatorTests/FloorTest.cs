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
            var floor = new Floor("A", 10);
            Assert.AreEqual("A", floor.Name);
            Assert.AreEqual(10, floor.ParkingLocations.Count);
            Assert.AreEqual("A-0", floor.ParkingLocations[0].Name);
            Assert.AreEqual("A-5", floor.ParkingLocations[5].Name);
            Assert.IsFalse(floor.IsFull);
        }

        [TestMethod]
        public void FullFloor()
        {
            var floor = new Floor("A", 5);
            foreach (var item in floor.ParkingLocations)
            {
                item.ParkAuto(new Auto("", 0));
            }
            Assert.IsTrue(floor.IsFull);
        }

        [TestMethod]
        public void FloorDepartAuto()
        {
            var floor = new Floor("A", 5);
            foreach (var item in floor.ParkingLocations)
            {
                item.ParkAuto(new Auto("", 0));
            }
            var auto = floor.ParkingLocations[3].AutoDeparts();
            Assert.IsNotNull(auto);
            Assert.IsFalse(floor.IsFull);
            Assert.IsNull(floor.ParkingLocations[3].Occupant);
        }


        [TestMethod]
        public void FloorOpenLocation()
        {
            var floor = new Floor("A", 5);
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
            var floor = new Floor("A", 5);
            floor.AutoEntering(new Auto("", .001));
            floor.Tick();
            Assert.AreEqual(1, floor.ParkingLocations.Where(r => r.Occupant != null).Count());

            Simulator.Clock.Tick();
            floor.Tick();
            Assert.AreEqual(0, floor.ParkingLocations.Where(r => r.Occupant != null).Count());
        }
    }
}
