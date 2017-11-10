using System;

namespace KeyWatcher.Reactive
{
	internal class EventedKeyWatcher
		: KeyWatcherBase
	{
		internal event EventHandler<KeyEventArgs> KeyLogged;

		protected override void HandleKey(char key) => 
			this.KeyLogged?.Invoke(this, new KeyEventArgs(key));
	}
}
