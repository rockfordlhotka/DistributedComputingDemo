using Akka.Actor;
using KeyWatcher.Messages;
using System;
using System.Web.Http;

namespace KeyWatcher.Azure.Controllers
{
	public sealed class KeyWatcherController
		: ApiController
	{
		private UsersActorHolder holder;

		public KeyWatcherController(UsersActorHolder holder)
		{
			if (holder == null)
			{
				throw new ArgumentNullException(nameof(holder));
			}

			this.holder = holder;
		}

		public void Post([FromBody] UserKeysMessage value)
		{
			this.holder.UsersActor.Tell(value);
		}
	}
}
