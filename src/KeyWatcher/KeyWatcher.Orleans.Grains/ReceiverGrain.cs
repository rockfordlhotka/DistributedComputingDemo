using KeyWatcher.Orleans.Contracts;
using Orleans;
using Orleans.Streams;
using System.Threading.Tasks;
using System;

namespace KeyWatcher.Orleans.Grains
{
	[ImplicitStreamSubscription("NotificationData")]
	public class ReceiverGrain 
		: Grain, IReceiverGrain, IAsyncObserver<string>
	{
		public override async Task OnActivateAsync()
		{
			var guid = this.GetPrimaryKey();
			var streamProvider = GetStreamProvider("NotificationStream");
			var stream = streamProvider.GetStream<string>(guid, "NotificationData");
			await stream.SubscribeAsync(this);
		}

		public Task OnCompletedAsync() => throw new NotImplementedException();

		public Task OnErrorAsync(Exception ex) => throw new NotImplementedException();

		public Task OnNextAsync(string item, StreamSequenceToken token = null)
		{
			Console.Out.WriteLine($"From receiver: {item}");
			return Task.CompletedTask;

		}
	}
}
