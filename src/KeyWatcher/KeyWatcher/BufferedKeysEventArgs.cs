using System;

namespace KeyWatcher
{
	public sealed class BufferedKeysEventArgs
		: EventArgs
	{
		public BufferedKeysEventArgs(char[] keys)
			: base()
		{
			this.Keys = keys;
		}

		public char[] Keys { get; }
	}
}