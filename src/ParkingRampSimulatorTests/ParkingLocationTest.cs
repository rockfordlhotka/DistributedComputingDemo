using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class ParkingLocationTest
    {
        [TestMethod]
        public void NewParkingLocation()
        {
            var location = new ParkingLocation("A");
            Assert.AreEqual("A", location.Name);
            Assert.IsNull(location.Occupant);
        }

        [TestMethod]
        public void ParkingLocationParkAuto()
        {
            var handler = new MessageHandler<ParkingLocation.AutoParkedMessage>();
            Simulator.Notifier.Subscribe(handler);
            var location = new ParkingLocation("A");
            var auto = new Auto("", 0);
            location.ParkAuto(auto);
            Assert.AreSame(auto, location.Occupant);
            Assert.AreEqual(1, handler.MessageCount);
            Assert.AreEqual(auto, handler.Message.Auto);
            Simulator.Notifier.UnsubscribeFromAll(handler);
        }

        [TestMethod]
        public void ParkingLocationAutoDeparts()
        {
            var handler = new MessageHandler<ParkingLocation.AutoDepartedMessage>();
            Simulator.Notifier.Subscribe(handler);
            var location = new ParkingLocation("A");
            var auto = new Auto("", 0);
            location.ParkAuto(auto);
            location.AutoDeparts();
            Assert.IsNull(location.Occupant);
            Assert.AreEqual(1, handler.MessageCount);
            Assert.AreEqual(auto, handler.Message.Auto);
            Simulator.Notifier.UnsubscribeFromAll(handler);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParkingLocationNullAutoDeparts()
        {
            var handler = new MessageHandler<ParkingLocation.AutoDepartedMessage>();
            Simulator.Notifier.Subscribe(handler);
            var location = new ParkingLocation("A");
            try
            {
                location.AutoDeparts();
            }
            finally
            {
                Assert.AreEqual(0, handler.MessageCount);
                Simulator.Notifier.UnsubscribeFromAll(handler);
            }
        }
    }
}
