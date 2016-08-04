using System;

namespace KeyWatcher
{
	public abstract class KeyWatcher
		: IDisposable
	{
		protected abstract void HandleKey(int keyCode);

		public abstract void Dispose();
	}
}
