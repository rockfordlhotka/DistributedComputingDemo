using System;
using System.Collections.Generic;

namespace KeyWatcher.Reactive
{
	internal sealed class ObservingCancellingKeyWatcher
		: IObserver<char>
	{
		private readonly string id;
		private readonly KeyWatcherBase keyWatcher;
		private readonly Queue<char> buffer = new Queue<char>();

		internal ObservingCancellingKeyWatcher(string id, KeyWatcherBase keyWatcher)
		{
			this.id = id;
			this.keyWatcher = keyWatcher;
		}

		public void OnCompleted() =>
			Console.Out.WriteLine($"{this.id} - {nameof(this.OnCompleted)}");

		public void OnError(Exception error) =>
			Console.Out.WriteLine($"{this.id} - {nameof(this.OnError)} - {error.Message}");

		public void OnNext(char value)
		{
			Console.Out.WriteLine($"{this.id} - {nameof(this.OnNext)} - {value}");
			if (this.CheckForTermination(value)) { this.keyWatcher.Cancel(); }
		}

		private bool CheckForTermination(char key)
		{
			this.buffer.Enqueue(key);

			while (this.buffer.Count > Program.Termination.Length)
			{
				this.buffer.Dequeue();
			}

			if (this.buffer.Count == Program.Termination.Length)
			{
				var termination = new string(this.buffer.ToArray());

				if (termination == Program.Termination)
				{
					return true;
				}
			}

			return false;
		}
	}
}
