using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class AutoTest
    {
        private const long TicksInADay = 864000000000;

        [TestMethod]
        public void NewAuto()
        {
            var auto = new Auto("abc123", 2.5);
            var now = Simulator.Clock.Now;
            Assert.AreEqual("abc123", auto.LicensePlate);

            var span = (long)(2.5 * TicksInADay);
            var dateToDepart = now.Add(new TimeSpan(span));
            Assert.AreEqual(dateToDepart, auto.DateToDepart);
        }
    }
}
