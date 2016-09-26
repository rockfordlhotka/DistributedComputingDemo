using Akka.Actor;

namespace KeyWatcher.Azure
{
	public sealed class UsersActorHolder
	{
		public IActorRef UsersActor { get; }

		public UsersActorHolder(IActorRef usersActor)
		{
			this.UsersActor = usersActor;
		}
	}
}