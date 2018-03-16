using KeyWatcher.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace KeyWatcher.Signaled.Client.Extensions
{
	public static class HubConnectionExtensions
	{
		public static IObservable<T> ObserveConnection<T>(this HubConnection @this, string method)
		{
			var observable = new HubConnectionObservable<T>();
			@this.On<T>(method, data => observable.On(data));
			return observable;
		}

		private sealed class HubConnectionObservable<T> : IObservable<T>
		{
			private readonly List<IObserver<T>> observers =
				new List<IObserver<T>>();

			public IDisposable Subscribe(IObserver<T> observer)
			{
				if (!this.observers.Contains(observer))
				{
					this.observers.Add(observer);
				}

				return Disposable.Empty;
			}

			public void On(T message)
			{
				foreach (var observer in this.observers)
				{
					observer.OnNext(message);
				}
			}
		}
	}
}
