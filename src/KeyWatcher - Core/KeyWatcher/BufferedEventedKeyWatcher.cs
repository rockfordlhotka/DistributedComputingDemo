using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace KeyWatcher
{
	public sealed class BufferedEventedKeyWatcher
		: KeyWatcherBase
	{
		public event EventHandler<BufferedKeysEventArgs> KeysLogged;

		private readonly ushort size;
		private List<char> buffer;

		public BufferedEventedKeyWatcher(ushort size)
		{
			this.buffer = new List<char>(size);
			this.size = size;
		}

		protected override void HandleKey(char key)
		{
			this.buffer.Add(Convert.ToChar(key));

			if (this.buffer.Count >= this.size)
			{
				this.KeysLogged?.Invoke(
					this, new BufferedKeysEventArgs(this.buffer.ToImmutableArray()));
				this.buffer.Clear();
			}
		}
	}
}
