using KeyWatching;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace KeyWatcher.Reactive
{
	// NOTE: You really shouldn't do this:
	// https://stackoverflow.com/questions/10480542/why-shouldnt-i-implement-iobservablet
	internal sealed class ObservableNativeKeyWatcher
		: NativeKeyWatcher, IObservable<char>
	{
		private readonly List<IObserver<char>> observers =
			new List<IObserver<char>>();

		public IDisposable Subscribe(IObserver<char> observer)
		{
			if(!this.observers.Contains(observer))
			{
				this.observers.Add(observer);
			}

			return Disposable.Empty;
		}

		protected override void HandleKey(int keyCode)
		{
			foreach(var observer in this.observers)
			{
				observer.OnNext(Convert.ToChar(keyCode));
			}
		}
	}
}
