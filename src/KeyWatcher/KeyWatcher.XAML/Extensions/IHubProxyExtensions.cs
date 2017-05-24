using Microsoft.AspNet.SignalR.Client;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace KeyWatcher.XAML.Extensions
{
	public static class IHubProxyExtensions
	{
		public static IObservable<T> ObserveAs<T>(this IHubProxy @this, string eventName) =>
			from item in @this.Observe(eventName)
			let m = item[0].ToObject<T>()
			select m;
	}
}
