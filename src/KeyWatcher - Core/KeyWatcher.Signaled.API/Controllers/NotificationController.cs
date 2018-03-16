using System;
using System.Threading.Tasks;
using KeyWatcher.Messages;
using Microsoft.AspNetCore.Mvc;

namespace KeyWatcher.Signaled.API.Controllers
{
	[Route("api/[controller]")]
	public sealed class NotificationController
		: Controller
	{
		public async Task Post([FromBody] NotificationMessage value) =>
			await Console.Out.WriteLineAsync($"Got notification message: {value.Message}");
	}
}
