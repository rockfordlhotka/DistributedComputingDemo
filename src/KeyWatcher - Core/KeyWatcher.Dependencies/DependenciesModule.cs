using Autofac;
using System.Net.Http;

namespace KeyWatcher.Dependencies
{
	public sealed class DependenciesModule
		: Module
	{
		private readonly bool useApiForNotification;

		public DependenciesModule(bool useApiForNotification) => 
			this.useApiForNotification = useApiForNotification;

		protected override void Load(ContainerBuilder builder)
		{
			if(this.useApiForNotification)
			{
				var client = new HttpClient();
				builder.RegisterType<APINotification>().As<INotification>();
			}
			else
			{
				builder.RegisterType<EmailNotification>().As<INotification>();
			}

			builder.RegisterType<Logger>().As<ILogger>();
		}
	}
}
