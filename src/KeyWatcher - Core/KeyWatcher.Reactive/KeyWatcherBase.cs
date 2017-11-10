using System;

namespace KeyWatcher.Reactive
{
	internal abstract class KeyWatcherBase
	{
		internal void Cancel() => this.IsCancelled = true;

		protected abstract void HandleKey(char key);

		internal void Listen()
		{
			while (true)
			{
				this.HandleKey(Console.ReadKey().KeyChar);

				if (this.IsCancelled) { break; }
			}
		}

		internal bool IsCancelled { get; private set; }
	}
}
