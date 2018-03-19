using Autofac;
using Autofac.Extensions.DependencyInjection;
using KeyWatcher.Dependencies;
using KeyWatcher.Orleans.Grains;
using KeyWatcher.Orleans.Host.StorageProviders;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Host
{
	public static class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = new SiloHostBuilder()
				.UseLocalhostClustering()
				.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
				.AddFileStorage(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME, 
					configureOptions => configureOptions.Configure(options => options.RootDirectory = @".\Storage"))
				//.AddMemoryGrainStorage("Default")
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