using Autofac;

namespace KeyWatcher.Actors
{
	public sealed class ActorsModule
		: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UsersActor>();
			builder.RegisterType<UserActor>();
			builder.RegisterType<EmailActor>();
		}
	}
}
