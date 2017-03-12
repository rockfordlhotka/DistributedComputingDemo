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
			// NOTE: This RemoveAt() line should be removed once this
			// is resolved: https://github.com/dotnet/orleans/issues/2747
			services.RemoveAt(0);
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule<DependenciesModule>();
			containerBuilder.Populate(services);
			var container = containerBuilder.Build();
			return new AutofacServiceProvider(container);
		}
	}
}
