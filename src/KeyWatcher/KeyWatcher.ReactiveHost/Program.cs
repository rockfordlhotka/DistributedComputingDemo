using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace KeyWatcher.ReactiveHost
{
	class Program
	{
		private static readonly char[] Keys = { 'X', 'Q', 'Z' };

		static void Main(string[] args)
		{
			using (var keyLogger = new EventedKeyWatcher())
			{
				using (var observable = Observable.FromEventPattern<KeyEventArgs>(
					keyLogger, nameof(EventedKeyWatcher.KeyLogged))
					.Where(pattern => Program.Keys.Contains(pattern.EventArgs.Key))
					.Take(10)
					.Subscribe(pattern => Console.Out.Write(pattern.EventArgs.Key)))
				{
					Application.Run();
				}
			}
		}
	}
}
