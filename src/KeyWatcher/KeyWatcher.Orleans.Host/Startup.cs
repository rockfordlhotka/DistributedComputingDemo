using Autofac;
using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KeyWatcher.Orleans.Host
{
	public class Startup
	{
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			//TODO: I'd love to do this, but it's not working: (
			//var containerBuilder = new ContainerBuilder();
			//containerBuilder.RegisterModule<DependenciesModule>();
			//containerBuilder.Populate(services);
			//var container = containerBuilder.Build();
			//return new AutofacServiceProvider(container);

			services.AddSingleton<ILogger, Logger>();
			services.AddSingleton<INotification, EmailNotification>();
			return services.BuildServiceProvider();
		}
	}
}
