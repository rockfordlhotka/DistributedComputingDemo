using System;
using System.Threading;
using System.Threading.Tasks;

namespace KeyWatcher
{
	public sealed class SimulatedKeyWatcher
		: KeyWatcher
	{
		public event EventHandler<KeyEventArgs> KeyLogged;

		private static string content = "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG.";
		private Task keyGenerator;
		private CancellationToken cancel;
		private CancellationTokenSource source;

		public SimulatedKeyWatcher()
			: base()
		{
			this.source = new CancellationTokenSource();
			this.cancel = source.Token;

			this.keyGenerator = Task.Factory.StartNew(() =>
			{
				var index = 0;
				var random = new Random();

				while (!this.cancel.IsCancellationRequested)
				{
					Task.Delay(random.Next(150, 225)).Wait();
					this.HandleKey(SimulatedKeyWatcher.content[index]);
					index = (index + 1) % content.Length;
				}
			});
		}

		public override void Dispose()
		{
			this.source.Cancel();
			this.keyGenerator.Wait();
		}

		protected override void HandleKey(int keyCode)
		{
			if (keyCode >= Char.MinValue && keyCode <= char.MaxValue)
			{
				this.KeyLogged?.Invoke(
					this, new KeyEventArgs(Convert.ToChar(keyCode)));
			}
		}
	}
}
