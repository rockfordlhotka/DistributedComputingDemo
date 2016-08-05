using Autofac;

namespace KeyWatcher.Dependencies
{
	public sealed class DependenciesModule
		: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Email>().As<IEmail>();
			builder.RegisterType<Logger>().As<ILogger>();
		}
	}
}
