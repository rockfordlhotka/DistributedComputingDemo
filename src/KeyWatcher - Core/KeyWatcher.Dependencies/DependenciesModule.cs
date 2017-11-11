using Autofac;

namespace KeyWatcher.Dependencies
{
	public sealed class DependenciesModule
		: Module
	{
		private readonly bool useSignalForNotification;

		public DependenciesModule(bool useSignalForNotification) => 
			this.useSignalForNotification = useSignalForNotification;

		protected override void Load(ContainerBuilder builder)
		{
			if(this.useSignalForNotification)
			{
				builder.RegisterType<SignalRNotification>().As<INotification>();
			}
			else
			{
				builder.RegisterType<EmailNotification>().As<INotification>();
			}

			builder.RegisterType<Logger>().As<ILogger>();
		}
	}
}
