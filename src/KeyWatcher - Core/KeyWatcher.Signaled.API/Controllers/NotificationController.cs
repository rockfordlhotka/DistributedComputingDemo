using System;
using System.Threading.Tasks;
using KeyWatcher.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KeyWatcher.Signaled.API.Controllers
{
	[Route("api/[controller]")]
	public sealed class NotificationController
		: Controller
	{
		private readonly IHubContext<KeyWatcherHub> context;

		public NotificationController(IHubContext<KeyWatcherHub> context) => 
			this.context = context ?? throw new ArgumentNullException(nameof(context));

		public async Task Post([FromBody] NotificationMessage value) =>
			await this.context.Clients.All.SendAsync(Common.NotificationSent, new NotificationMessage(value.Recipient, value.Title, value.Message));
	}
}
