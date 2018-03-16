using Autofac;
using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Dependencies;
using KeyWatcher.Orleans.Grains;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KeyWatcher.Signaled.Silo
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var builder = new SiloHostBuilder()
				.UseLocalhostClustering()
				.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
				.AddMemoryGrainStorage("Default")
				.UseServiceProviderFactory(services =>
				{
					var containerBuilder = new ContainerBuilder();
					containerBuilder.RegisterModule(new DependenciesModule(true));
					containerBuilder.Populate(services);
					var container = containerBuilder.Build();
					return new AutofacServiceProvider(container);
				})
				.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences());

			var host = builder.Build();
			await host.StartAsync();

			await Console.Out.WriteLineAsync("Orleans silo is running.");
			await Console.Out.WriteLineAsync("Press Enter to terminate...");
			await Console.In.ReadLineAsync();

			await host.StopAsync();
			await Console.Out.WriteLineAsync("Orleans silo is terminated.");
		}
	}
}
