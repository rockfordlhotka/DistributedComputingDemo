using System;

namespace KeyWatcher.Reactive
{
	internal sealed class KeyEventArgs 
		: EventArgs
	{
		internal KeyEventArgs(char key) => this.Key = key;

		internal char Key { get; }
	}
}
