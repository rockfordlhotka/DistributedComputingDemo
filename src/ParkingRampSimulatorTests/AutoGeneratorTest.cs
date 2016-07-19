using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class AutoGeneratorTest
    {
        [TestMethod]
        public void GenerateAuto()
        {
            var generator = new AutoGenerator();
            var auto = generator.GenerateAuto();
            Assert.AreEqual(6, auto.LicensePlate.Length);
            Assert.IsTrue(auto.DateToDepart > Simulator.Clock.Now);
        }

        [TestMethod]
        public void AutoGeneratorTick()
        {
            var generator = new AutoGenerator();
            generator.Tick();
        }
    }
}
