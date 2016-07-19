using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class ClockTest
    {
        [TestMethod]
        public void ClockTick()
        {
            var interval = new TimeSpan(0, 7, 17);
            var clock = new Clock(interval);
            var now = clock.Now;
            clock.Tick();
            Assert.AreNotEqual(now, DateTime.Now);
            var then = now.Add(interval);
            Assert.AreEqual(then, clock.Now);
        }
    }
}
