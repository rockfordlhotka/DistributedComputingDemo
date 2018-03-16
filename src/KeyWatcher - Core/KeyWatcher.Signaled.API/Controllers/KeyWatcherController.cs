using System;
using System.Threading.Tasks;
using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace KeyWatcher.Signaled.API.Controllers
{
	[Route("api/[controller]")]
	public sealed class KeyWatcherController
		: Controller
	{
		private readonly Func<string, IUserGrain> userGrainFactory;

		public KeyWatcherController(Func<string, IUserGrain> userGrainFactory) =>
			this.userGrainFactory = userGrainFactory ?? throw new ArgumentNullException(nameof(userGrainFactory));

		public async Task Post([FromBody] UserKeysMessage value) =>
			await this.userGrainFactory(value.Name).ProcessAsync(value);
	}
}
