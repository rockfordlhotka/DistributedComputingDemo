using System;

namespace KeyWatcher
{
	public sealed class KeyEventArgs
		: EventArgs
	{
		public KeyEventArgs(char key)
			: base()
		{
			this.Key = key;
		}

		public char Key { get; }
	}
}