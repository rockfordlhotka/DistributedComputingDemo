using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class ParkingFacilityTest
    {
        [TestMethod]
        public void NewParkingFacility()
        {
            var obj = new ParkingFacility();
            Assert.IsNotNull(obj.ParkingRamps);
            Assert.IsTrue(obj.IsFull);
            obj.ParkingRamps.Add(new ParkingRamp(obj, "Red", 1, 100));
            Assert.IsFalse(obj.IsFull);
        }

        [TestMethod]
        public void ParkingFacilityGetOpenRamp()
        {
            var obj = new ParkingFacility();
            obj.ParkingRamps.Add(new ParkingRamp(obj, "Red", 1, 50));
            obj.ParkingRamps.Add(new ParkingRamp(obj, "Blue", 5, 50));
            var ramp = obj.GetOpenRamp();
            Assert.IsNotNull(ramp);
        }
    }
}
