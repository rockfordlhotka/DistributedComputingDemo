using Autofac;
using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Dependencies;
using KeyWatcher.Orleans.Grains;
using KeyWatcher.Orleans.Host.StorageProviders;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Host
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			var configuration = ClusterConfiguration.LocalhostPrimarySilo();
			configuration.Globals.RegisterStorageProvider(typeof(FileStorageProvider).FullName, "Default",
				new Dictionary<string, string>() { { "RootDirectory", @".\Storage" } });
			//configuration.AddMemoryStorageProvider("Default");
			configuration.AddMemoryStorageProvider("PubSubStore");
			configuration.AddSimpleMessageStreamProvider("NotificationStream");

			var builder = new SiloHostBuilder()
				.UseConfiguration(configuration)
				//.ConfigureLogging(logging => logging.AddConsole())
				.UseServiceProviderFactory(services =>
				{
					var containerBuilder = new ContainerBuilder();
					containerBuilder.RegisterModule<DependenciesModule>();
					containerBuilder.Populate(services);
					var container = containerBuilder.Build();
					return new AutofacServiceProvider(container);
				})
				.AddApplicationPartsFromReferences(typeof(UserGrain).Assembly);

			var host = builder.Build();
			await host.StartAsync();

			await Console.Out.WriteLineAsync("Orleans silo is running.");
			await Console.Out.WriteLineAsync("Press Enter to terminate...");
			Console.ReadLine();

			await host.StopAsync();
			await Console.Out.WriteLineAsync("Orleans silo is terminated.");
		}
	}
}
