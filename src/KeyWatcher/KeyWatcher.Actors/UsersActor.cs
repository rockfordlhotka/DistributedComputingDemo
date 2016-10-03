using System;
using Akka.Actor;
using KeyWatcher.Messages;
using System.Collections.Generic;
using Akka.DI.Core;

namespace KeyWatcher.Actors
{
	public sealed class UsersActor
		: ReceiveActor
	{
		private Dictionary<string, IActorRef> users = new Dictionary<string, IActorRef>();

		public UsersActor()
		{
			this.Receive<UserKeysMessage>(message => this.Handle(message));
		}

		private void Handle(UserKeysMessage message)
		{
			if (this.users.ContainsKey(message.Name))
			{
				this.users[message.Name].Tell(message);
			}
			else
			{
				var user = ActorBase.Context.ActorOf(
					ActorBase.Context.DI().Props<UserActor>(), message.Name);
				this.users.Add(message.Name, user);
				Console.Out.WriteLine($"New user at {user.Path.ToStringWithAddress()}");
				user.Tell(message);
			}
		}
	}
}
