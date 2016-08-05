using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParkingRampSimulatorServices.Contracts
{
    public class FacilityStatus
    {
        public int TotalLocations { get; set; }
        public int OpenLocations { get; set; }
        public double PercentageOpen { get; set; }
    }
}