using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingRampSimulator
{
    public class Notifier
    {
        private class Subscription
        {
            public object Subscriber { get; set; }
            public Type SubscriptionType { get; set; }
        }

        private List<Subscription> _subscriptions = new List<Subscription>();

        public void Subscribe<M>(ISubscriber<M> subscriber)
        {
            _subscriptions.Add(new Subscription { Subscriber = subscriber, SubscriptionType = typeof(M) });
        }

        public bool IsSubscribed<M>(ISubscriber<M> subscriber)
        {
            var subs = _subscriptions.Where(r => ReferenceEquals(r.Subscriber, subscriber) && r.SubscriptionType.Equals(typeof(M)));
            return subs.Count() > 0;
        }

        public void Unsubscribe<M>(ISubscriber<M> subscriber)
        {
            var subs = _subscriptions.Where(r => ReferenceEquals(r.Subscriber, subscriber) && r.SubscriptionType.Equals(typeof(M)));
            foreach (var item in subs.ToList())
                _subscriptions.Remove(item);
        }

        public void UnsubscribeFromAll(object subscriber)
        {
            var subs = _subscriptions.Where(r => ReferenceEquals(r.Subscriber, subscriber));
            foreach (var item in subs.ToList())
                _subscriptions.Remove(item);
        }

        public void Notify<M>(M message)
        {
            var subs = _subscriptions.Where(r => r.SubscriptionType.Equals(typeof(M)));
            foreach (var item in subs.ToList())
                ((ISubscriber<M>)item.Subscriber).HandleMessage(message);
        }
    }

    public interface ISubscriber<M>
    {
        void HandleMessage(M message);
    }
}
