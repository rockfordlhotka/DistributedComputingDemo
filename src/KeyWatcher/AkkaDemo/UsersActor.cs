using Akka.Actor;
using System;
using System.Collections.Generic;

namespace AkkaDemo
{
	public sealed class UsersActor
		: ReceiveActor
	{
		private Dictionary<uint, IActorRef> users = new Dictionary<uint, IActorRef>();

		public UsersActor()
		{
			this.Receive<UserCountMessage>(message => this.Handle(message));
		}

		private void Handle(UserCountMessage message)
		{
			if(this.users.ContainsKey(message.Id))
			{
				this.users[message.Id].Tell(message.Count);
			}
			else
			{
				// Even IDs are "regular" actors,
				// Odd IDs are persisted actors.
				var user = message.Id % 2 == 0 ?
					ActorBase.Context.ActorOf(
						Props.Create<UserActor>(message.Id), message.Id.ToString()) :
					ActorBase.Context.ActorOf(
						Props.Create<PersistentUserActor>(message.Id), message.Id.ToString());
				this.users.Add(message.Id, user);
				Console.Out.WriteLine($"New user at {user.Path.ToStringWithAddress()}");
				user.Tell(message.Count);
			}
		}
	}
}