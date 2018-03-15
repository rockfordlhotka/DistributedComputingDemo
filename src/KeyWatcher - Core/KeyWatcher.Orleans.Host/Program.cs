using Autofac;
using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Dependencies;
using KeyWatcher.Orleans.Grains;
using KeyWatcher.Orleans.Host.StorageProviders;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Host
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			//var configuration = ClusterConfiguration.LocalhostPrimarySilo();
			//configuration.Globals.RegisterStorageProvider(typeof(FileStorageProvider).FullName, "Default",
			//	new Dictionary<string, string>() { { "RootDirectory", @".\Storage" } });
			////configuration.AddMemoryStorageProvider("Default");
			//configuration.AddMemoryStorageProvider("PubSubStore");
			//configuration.AddSimpleMessageStreamProvider("NotificationStream");

			var builder = new SiloHostBuilder()
				.UseLocalhostClustering()
				.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
				.AddMemoryGrainStorage("Default")
				//.ConfigureLogging(logging => logging.AddConsole())
				.UseServiceProviderFactory(services =>
				{
					var containerBuilder = new ContainerBuilder();
					containerBuilder.RegisterModule(new DependenciesModule(false));
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
