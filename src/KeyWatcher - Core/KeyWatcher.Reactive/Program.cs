using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace KeyWatcher.Reactive
{
	public static class Program
	{
		private const ushort BufferCount = 20;
		private const ushort BufferSkip = 16;
		internal const string Termination = "STOP IT";

		private static Queue<char> buffer = new Queue<char>();

		static void Main(string[] args) =>
			//Program.HandleKeysViaEvents();
			//Program.HandleKeysViaManualObservable();
			//Program.HandleKeysFromEventPattern();
			Program.HandleKeysFromEventPatternWithOperators();

		private static void HandleKeysViaEvents()
		{
			var keyLogger = new EventedKeyWatcher();
			keyLogger.KeyLogged += (s, e) =>
			{
				Console.Write(e.Key);
				if (Program.CheckForTermination(e.Key)) { keyLogger.Cancel(); }
			};
			keyLogger.Listen();
		}

		private static void HandleKeysViaManualObservable()
		{
			var keyLogger = new ObservableKeyWatcher();
			keyLogger.Subscribe(new ObservingKeyWatcher("Observer 1"));
			keyLogger.Subscribe(new ObservingKeyWatcher("Observer 2"));
			keyLogger.Subscribe(new ObservingCancellingKeyWatcher("Observer 3", keyLogger));
			keyLogger.Listen();
		}

		private static void HandleKeysFromEventPattern()
		{
			var keyLogger = new EventedKeyWatcher();
			var observable = Observable.FromEventPattern<KeyEventArgs>(
				keyLogger, nameof(EventedKeyWatcher.KeyLogged));
			using (var subscription = observable.Subscribe(
				pattern =>
				{
					var key = pattern.EventArgs.Key;
					Console.Out.Write(key);
					if (Program.CheckForTermination(key)) { keyLogger.Cancel(); }
				}))
			{
				keyLogger.Listen();
			}
		}

		private static void HandleKeysFromEventPatternWithOperators()
		{
			var keyLogger = new EventedKeyWatcher();
			var observable = Observable.FromEventPattern<KeyEventArgs>(
				keyLogger, nameof(EventedKeyWatcher.KeyLogged));
			var operationObservable = observable
				.Select(e => e.EventArgs.Key)
				.Buffer(Program.BufferCount, Program.BufferSkip)
				.Delay(TimeSpan.FromSeconds(2));

			using (var subscription = observable.Subscribe(
				pattern =>
				{
					if (Program.CheckForTermination(pattern.EventArgs.Key)) { keyLogger.Cancel(); }
				}))
			{
				using (var operationSubscription = operationObservable.Subscribe(
					operationPattern =>
					{
						Console.Out.WriteLine(operationPattern.ToArray());
					}))
				{
					keyLogger.Listen();
				}
			}
		}

		private static bool CheckForTermination(char key)
		{
			Program.buffer.Enqueue(key);

			while (Program.buffer.Count > Program.Termination.Length)
			{
				Program.buffer.Dequeue();
			}

			if (Program.buffer.Count == Program.Termination.Length)
			{
				var termination = new string(Program.buffer.ToArray());

				if (termination == Program.Termination)
				{
					return true;
				}
			}

			return false;
		}
	}
}
