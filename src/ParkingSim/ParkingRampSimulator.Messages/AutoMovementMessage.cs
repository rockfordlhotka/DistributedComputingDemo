using System;

namespace ParkingRampSimulator.Messages
{
    public class AutoMovementMessage : Message
    {
        public AutoMovementMessage(DateTime timeStamp)
            : base(timeStamp)
        { }

        public string LicensePlate { get; set; }
        public string Location { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            var location = Location;
            if (string.IsNullOrWhiteSpace(location))
                location = "facility";
            return string.Format("{0}: {1} at {2} departing {3}, Status: {4}", TimeStamp, LicensePlate, location, DepartureDate, Status);
        }
    }
}
