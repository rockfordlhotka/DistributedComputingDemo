using System;

namespace KeyWatcher.Reactive
{
	internal sealed class ObservingKeyWatcher
		: IObserver<char>
	{
		private readonly string id;

		internal ObservingKeyWatcher(string id) =>
			this.id = id;

		public void OnCompleted() =>
			Console.Out.WriteLine($"{this.id} - {nameof(this.OnCompleted)}");

		public void OnError(Exception error) =>
			Console.Out.WriteLine($"{this.id} - {nameof(this.OnError)} - {error.Message}");

		public void OnNext(char value) =>
			Console.Out.WriteLine($"{this.id} - {nameof(this.OnNext)} - {value}");
	}
}
