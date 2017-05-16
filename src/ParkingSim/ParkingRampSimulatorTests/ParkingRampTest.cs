using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class ParkingRampTest
    {
        [TestMethod]
        public void NewParkingRamp()
        {
            var ramp = new ParkingRamp(null, "Red", 4, 100);
            Assert.AreEqual(4, ramp.Floors.Count);
            Assert.AreEqual("Red", ramp.Name);
            Assert.AreEqual("Red-1", ramp.Floors[1].Name);
        }

        [TestMethod]
        public void RampTick()
        {
            var ramp = new ParkingRamp(null, "Red", 4, 100);
            ramp.InQueue.Enqueue(new Auto("", 1));
            ramp.Tick();
        }
    }
}
