using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Orleans.Grains;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Threading.Tasks;

namespace KeyWatcher.Azure
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			var webHost = Program.BuildWebHost(args);
			var siloHost = Program.BuildSiloHost(webHost.Services);
			await siloHost.StartAsync();
			await webHost.RunAsync();
			await siloHost.StopAsync();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureServices(services => services.AddAutofac())
				.UseStartup<Startup>()
				.Build();

		private static ISiloHost BuildSiloHost(IServiceProvider serviceProvider)
		{
			var configuration = ClusterConfiguration.LocalhostPrimarySilo();
			configuration.AddMemoryStorageProvider("Default");
			configuration.AddMemoryStorageProvider("PubSubStore");
			configuration.AddSimpleMessageStreamProvider("NotificationStream");

			return new SiloHostBuilder()
				.UseConfiguration(configuration)
				.UseServiceProviderFactory(services => serviceProvider)
				.AddApplicationPartsFromReferences(typeof(UserGrain).Assembly)
				.Build();
		}
	}
}
