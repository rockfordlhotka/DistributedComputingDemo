using Autofac;
using Microsoft.AspNetCore.SignalR;
using System;

namespace KeyWatcher.Dependencies
{
	public sealed class DependenciesModule
		: Module
	{
		private readonly IHubContext<KeyWatcherHub, IKeyWatcherHub> hub;

		public DependenciesModule()
			: base()
		{
		}

		public DependenciesModule(IHubContext<KeyWatcherHub, IKeyWatcherHub> hub) =>
			this.hub = hub ?? throw new ArgumentNullException(nameof(hub));

		protected override void Load(ContainerBuilder builder)
		{
			if(this.hub != null)
			{
				builder.RegisterInstance(this.hub).As<IHubContext<KeyWatcherHub, IKeyWatcherHub>>();
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
