using Akka.Actor;
using Autofac;

namespace KeyWatcher.Azure
{
	internal sealed class AzureModule
		: Module
	{
		private IActorRef usersActor;

		public AzureModule(IActorRef usersActor)
		{
			this.usersActor = usersActor;
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterInstance<UsersActorHolder>(new UsersActorHolder(this.usersActor));
		}
	}
}