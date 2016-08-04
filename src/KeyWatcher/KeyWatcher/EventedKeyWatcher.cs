using System;

namespace KeyWatcher
{
	public sealed class EventedKeyWatcher
		: Win32KeyWatcher
	{
		public event EventHandler<KeyEventArgs> KeyLogged;

		protected override void HandleKey(int keyCode)
		{
			if(keyCode >= Char.MinValue && keyCode <= char.MaxValue)
			{
				this.KeyLogged?.Invoke(
					this, new KeyEventArgs(Convert.ToChar(keyCode)));
			}
		}
	}
}
