using Autofac;
using System;
using System.Net.Http;

namespace KeyWatcher.Dependencies
{
	public sealed class DependenciesModule
		: Module
	{
		private readonly bool useApiForNotification;
		private readonly Uri apiUri;

		public DependenciesModule(bool useApiForNotification, Uri apiUri = null)
		{
			this.apiUri = apiUri;
			this.useApiForNotification = useApiForNotification;
		}

		protected override void Load(ContainerBuilder builder)
		{
			if(this.useApiForNotification)
			{
				builder.RegisterInstance(new APINotification(new HttpClient(), this.apiUri)).As<INotification>();
			}
			else
			{
				builder.RegisterType<EmailNotification>().As<INotification>();
			}

			builder.RegisterType<Logger>().As<ILogger>();
		}
	}
}
