using KeyWatcher.Messages;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	internal sealed class SignalRNotification
		: INotification
	{
		private readonly IHubContext<KeyWatcherHub, IKeyWatcherHub> hub;

		public SignalRNotification(IHubContext<KeyWatcherHub, IKeyWatcherHub> hub) =>
			this.hub = hub ?? throw new ArgumentNullException(nameof(hub));

		public Task SendAsync(string recipient, string title, string message)
		{
			this.hub.Clients.All.NotificationSent(
				new SignalRNotificationMessage(recipient, title, message));
			return Task.CompletedTask;
		}
	}
}
