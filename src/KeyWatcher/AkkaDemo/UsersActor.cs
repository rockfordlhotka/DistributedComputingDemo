using Akka.Actor;
using System;
using System.Collections.Generic;

namespace AkkaDemo
{
	public sealed class UsersActor
		: TypedActor, IHandle<UserCountMessage>
	{
		private Dictionary<uint, IActorRef> users = new Dictionary<uint, IActorRef>();

		public void Handle(UserCountMessage message)
		{
			if(this.users.ContainsKey(message.Id))
			{
				this.users[message.Id].Tell(message.Count);
			}
			else
			{
				var user = ActorBase.Context.ActorOf(
					Props.Create<UserActor>(message.Id), message.Id.ToString());
				this.users.Add(message.Id, user);
				Console.Out.WriteLine($"New user at {user.Path.ToStringWithAddress()}");
				user.Tell(message.Count);
			}
		}
	}
}
