using System;

namespace KeyWatcher
{
	public abstract class KeyWatcherBase
	{
		public void Cancel() => this.IsCancelled = true;

		protected abstract void HandleKey(char key);

		public void Listen()
		{
			while (true)
			{
				this.HandleKey(Console.ReadKey().KeyChar);

				if (this.IsCancelled) { break; }
			}
		}

		public bool IsCancelled { get; private set; }
	}
}
