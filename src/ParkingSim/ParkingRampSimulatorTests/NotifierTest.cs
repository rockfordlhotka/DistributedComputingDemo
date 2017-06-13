using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingRampSimulator;

namespace ParkingRampSimulatorTests
{
    [TestClass]
    public class NotifierTest
    {
        public class MyMessage
        {
            public int MyProperty { get; set; }
        }

        public class MyMessage2
        {
            public int MyProperty { get; set; }
        }

        public class MyMessageHandler : ISubscriber<MyMessage>
        {
            public MyMessage MyMessage { get; set; }

            public void HandleMessage(MyMessage message)
            {
                MyMessage = message;
            }
        }

        public class MyMultipleMessageHandler : ISubscriber<MyMessage>, ISubscriber<MyMessage2>
        {
            public MyMessage MyMessage { get; set; }
            public MyMessage2 MyMessage2 { get; set; }

            public void HandleMessage(MyMessage2 message)
            {
                MyMessage2 = message;
            }

            public void HandleMessage(MyMessage message)
            {
                MyMessage = message;
            }
        }

        [TestMethod]
        public void NotifierSubscribe()
        {
            var handler = new MyMessageHandler();
            Simulator.Notifier.Subscribe(handler);
            Assert.IsTrue(Simulator.Notifier.IsSubscribed(handler));
            Simulator.Notifier.Unsubscribe(handler);
            Assert.IsFalse(Simulator.Notifier.IsSubscribed(handler));

            Simulator.Notifier.Subscribe(handler);
            Assert.IsTrue(Simulator.Notifier.IsSubscribed(handler));
            Simulator.Notifier.UnsubscribeFromAll(handler);
            Assert.IsFalse(Simulator.Notifier.IsSubscribed(handler));
        }

        [TestMethod]
        public void NotifierMultiSubscribe()
        {
            var handler = new MyMultipleMessageHandler();
            Simulator.Notifier.Subscribe<MyMessage>(handler);
            Assert.IsTrue(Simulator.Notifier.IsSubscribed<MyMessage>(handler));
            Simulator.Notifier.Unsubscribe<MyMessage>(handler);
            Assert.IsFalse(Simulator.Notifier.IsSubscribed<MyMessage>(handler));

            Simulator.Notifier.Subscribe<MyMessage>(handler);
            Simulator.Notifier.Subscribe<MyMessage2>(handler);
            Assert.IsTrue(Simulator.Notifier.IsSubscribed<MyMessage>(handler));
            Assert.IsTrue(Simulator.Notifier.IsSubscribed<MyMessage2>(handler));
            Simulator.Notifier.UnsubscribeFromAll(handler);
            Assert.IsFalse(Simulator.Notifier.IsSubscribed<MyMessage>(handler));
            Assert.IsFalse(Simulator.Notifier.IsSubscribed<MyMessage2>(handler));
        }

        [TestMethod]
        public void NotifierSendMessage()
        {
            var handler = new MyMessageHandler();
            Simulator.Notifier.Subscribe(handler);
            Simulator.Notifier.Notify(new MyMessage { MyProperty = 123 });
            Assert.AreEqual(123, handler.MyMessage.MyProperty);
            Simulator.Notifier.UnsubscribeFromAll(handler);
        }

        [TestMethod]
        public void NotifierMultiSendMessage()
        {
            var handler = new MyMultipleMessageHandler();
            Simulator.Notifier.Subscribe<MyMessage>(handler);
            Simulator.Notifier.Subscribe<MyMessage2>(handler);
            Simulator.Notifier.Notify(new MyMessage { MyProperty = 123 });
            Simulator.Notifier.Notify(new MyMessage2 { MyProperty = 123 });
            Assert.AreEqual(123, handler.MyMessage.MyProperty);
            Assert.AreEqual(123, handler.MyMessage2.MyProperty);
            Simulator.Notifier.UnsubscribeFromAll(handler);
        }
    }
}
