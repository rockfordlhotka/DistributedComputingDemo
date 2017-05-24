using Autofac;
using Microsoft.AspNet.SignalR;
using System;

namespace KeyWatcher.Dependencies
{
	public sealed class DependenciesModule
		: Module
	{
		private readonly IHubContext hub;

		public DependenciesModule()
			: base()
		{ }

		public DependenciesModule(IHubContext hub) =>
			this.hub = hub ?? throw new ArgumentNullException(nameof(hub));

		protected override void Load(ContainerBuilder builder)
		{
			if(this.hub != null)
			{
				builder.RegisterInstance(this.hub).As<IHubContext>();
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
