using Akka.Actor;
using KeyWatcher.Actors.Messages;
using System;

namespace KeyWatcher.Actors
{
	public sealed class UserActor
		: TypedActor, IHandle<UserKeys>
	{
		public void Handle(UserKeys message)
		{
			Console.WriteLine(message.Keys);
		}
	}
}
