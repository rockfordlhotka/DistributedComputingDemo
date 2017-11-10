using System;
using System.Collections.Generic;

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
			Program.HandleKeysViaManualObservable();

		private static void HandleKeysViaEvents()
		{
			var keyLogger = new EventedKeyWatcher();
			keyLogger.KeyLogged += (s, e) =>
			{
				Console.Write(e.Key);
				if(Program.CheckForTermination(e.Key)) { keyLogger.Cancel(); }
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
