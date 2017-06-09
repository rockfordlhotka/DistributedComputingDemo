using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using System;
using System.Web.Http;

namespace KeyWatcher.Azure.Controllers
{
	public sealed class KeyWatcherController
		: ApiController
	{
		private readonly Func<string, IUserGrain> userGrainFactory;

		public KeyWatcherController(Func<string, IUserGrain> userGrainFactory) =>
			this.userGrainFactory = userGrainFactory;

		public void Post([FromBody] UserKeysMessage value) =>
			this.userGrainFactory(value.Name).ProcessAsync(value);
	}
}
