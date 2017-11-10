using KeyWatcher.Orleans.Contracts;
using KeyWatcher.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KeyWatcher.Azure.Controllers
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
