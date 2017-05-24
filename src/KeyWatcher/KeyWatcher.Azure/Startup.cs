using Autofac;
using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using System;

namespace KeyWatcher.Azure
{
	public sealed class Startup
	{
		public void Configuration(IAppBuilder app) =>
			app.MapSignalR();

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule<DependenciesModule>();
			containerBuilder.Populate(services);
			var container = containerBuilder.Build();
			return new AutofacServiceProvider(container);
		}
	}
}