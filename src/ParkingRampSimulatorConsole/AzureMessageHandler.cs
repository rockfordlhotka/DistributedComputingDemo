using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ParkingRampSimulator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulatorConsole
{
    public class AzureMessageHandler<M> : ISubscriber<M>
    {
        static string ConnectionString = Config.GetQueueConnectionString();
        static string QueueName = "eventstream";

        public void HandleMessage(M message)
        {
            if (!string.IsNullOrWhiteSpace(message.ToString()))
            {
                var outmessage = ConverterExtensions.ConvertToMessage(message);
                WriteToQueue(outmessage);
                WriteToAllEvents(outmessage);
                var csm = message as ParkingConstruct.ConstructStatusMessage;
                if (csm != null && csm.Construct.Name.Length == 0)
                    WriteToFacilityEvents(outmessage);
            }
        }

        private void WriteToFacilityEvents(object message)
        {
            var client = TopicClient.CreateFromConnectionString(ConnectionString, "facilityevents");
            using (var buffer = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(buffer))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        ser.Serialize(jsonWriter, message);
                        jsonWriter.Flush();
                        buffer.Position = 0;
                        var outMessage = new BrokeredMessage(buffer.ToArray());
                        client.Send(outMessage);
                    }
                }
            }
        }

        private void WriteToAllEvents(object message)
        {
            var client = TopicClient.CreateFromConnectionString(ConnectionString, "allevents");
            using (var buffer = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(buffer))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        ser.Serialize(jsonWriter, message);
                        jsonWriter.Flush();
                        buffer.Position = 0;
                        var outMessage = new BrokeredMessage(buffer.ToArray());
                        client.Send(outMessage);
                    }
                }
            }
        }

        private static void WriteToQueue(object message)
        {
            var client = QueueClient.CreateFromConnectionString(ConnectionString, QueueName);
            using (var buffer = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(buffer))
                {
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        ser.TypeNameHandling = TypeNameHandling.All;
                        ser.Serialize(jsonWriter, message);
                        jsonWriter.Flush();
                        buffer.Position = 0;
                        var outMessage = new BrokeredMessage(buffer.ToArray());
                        var length = buffer.Length;
                        client.Send(outMessage);
                    }
                }
            }
        }
    }

    internal static class ConverterExtensions
    {
        public static object ConvertToMessage<M>(M message)
        {
            object result = null;
            result = ConvertToMessage(message as ParkingConstruct.ConstructStatusMessage);
            if (result != null) return result;
            result = ConvertToMessage(message as Clock.ClockTick);
            if (result != null) return result;
            result = ConvertToMessage(message as Notifier.NotificationException);
            if (result != null) return result;
            result = ConvertToMessage(message as ParkingFacility.AutoAbandoningFacility);
            if (result != null) return result;
            result = ConvertToMessage(message as ParkingFacility.AutoArrivingAtFacility);
            if (result != null) return result;
            result = ConvertToMessage(message as ParkingFacility.AutoDepartingFacility);
            if (result != null) return result;
            result = ConvertToMessage(message as ParkingLocation.AutoDeparted);
            if (result != null) return result;
            result = ConvertToMessage(message as ParkingLocation.AutoParked);
            if (result != null) return result;
            result = ConvertToMessage(message as SimulatorStatus);
            if (result != null) return result;

            throw new InvalidOperationException("ConvertToMessage");
        }

        public static ParkingRampSimulator.Messages.ConstructStatusMessage ConvertToMessage(this ParkingRampSimulator.ParkingConstruct.ConstructStatusMessage source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.ConstructStatusMessage(Simulator.Clock.Now)
            {
                Name = source.Construct.Name,
                TotalLocations = source.Construct.TotalLocations,
                OpenLocations = source.Construct.OpenLocations,
                InQueueLength = source.Construct.InQueueLength,
                OutQueueLength = source.Construct.OutQueueLength
            };
        }

        public static ParkingRampSimulator.Messages.ClockTickMessage ConvertToMessage(this Clock.ClockTick source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.ClockTickMessage(Simulator.Clock.Now)
            {
            };
        }

        public static ParkingRampSimulator.Messages.ExceptionMessage ConvertToMessage(this Notifier.NotificationException source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.ExceptionMessage(Simulator.Clock.Now)
            {
                Exception = source.Exception
            };
        }

        public static ParkingRampSimulator.Messages.AutoMovementMessage ConvertToMessage(this ParkingFacility.AutoAbandoningFacility source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.AutoMovementMessage(Simulator.Clock.Now)
            {
                LicensePlate=source.Auto.LicensePlate,
                DepartureDate = source.Auto.DateToDepart,
                Status = source.GetType().Name,
                Location = string.Empty
            };
        }

        public static ParkingRampSimulator.Messages.AutoMovementMessage ConvertToMessage(this ParkingFacility.AutoArrivingAtFacility source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.AutoMovementMessage(Simulator.Clock.Now)
            {
                LicensePlate = source.Auto.LicensePlate,
                DepartureDate = source.Auto.DateToDepart,
                Status = source.GetType().Name,
                Location = string.Empty
            };
        }

        public static ParkingRampSimulator.Messages.AutoMovementMessage ConvertToMessage(this ParkingFacility.AutoDepartingFacility source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.AutoMovementMessage(Simulator.Clock.Now)
            {
                LicensePlate = source.Auto.LicensePlate,
                DepartureDate = source.Auto.DateToDepart,
                Status = source.GetType().Name,
                Location = string.Empty
            };
        }

        public static ParkingRampSimulator.Messages.AutoMovementMessage ConvertToMessage(this ParkingLocation.AutoDeparted source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.AutoMovementMessage(Simulator.Clock.Now)
            {
                LicensePlate = source.Auto.LicensePlate,
                DepartureDate = source.Auto.DateToDepart,
                Status = source.GetType().Name,
                Location = source.Location.Name
            };
        }

        public static ParkingRampSimulator.Messages.AutoMovementMessage ConvertToMessage(this ParkingLocation.AutoParked source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.AutoMovementMessage(Simulator.Clock.Now)
            {
                LicensePlate = source.Auto.LicensePlate,
                DepartureDate = source.Auto.DateToDepart,
                Status = source.GetType().Name,
                Location = source.Location.Name
            };
        }

        public static ParkingRampSimulator.Messages.SimulatorStatusMessage ConvertToMessage(this SimulatorStatus source)
        {
            if (source == null) return null;
            return new ParkingRampSimulator.Messages.SimulatorStatusMessage(Simulator.Clock.Now)
            {
                RealTime = source.RealTime,
                ClockTime = source.SimulatorTime,
                Status = source.Status
            };
        }
    }
}
