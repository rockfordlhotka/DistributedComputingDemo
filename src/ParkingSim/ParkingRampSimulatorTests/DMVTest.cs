using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class DMVTest
    {
        [TestInitialize]
        public void Initialize()
        {
            DMV.Plates.Clear();
        }

        [TestMethod]
        public void GetLicensePlate()
        {
            var plate = DMV.GetPlate();
            Assert.AreEqual(6, plate.Length);
            Assert.AreEqual(1, DMV.Plates.Count);
            Assert.AreEqual(plate, DMV.Plates[0]);
        }
    }
}
