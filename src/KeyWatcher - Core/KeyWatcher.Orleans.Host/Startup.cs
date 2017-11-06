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
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule<DependenciesModule>();
			containerBuilder.Populate(services);
			var container = containerBuilder.Build();
			return new AutofacServiceProvider(container);
		}
	}
}
