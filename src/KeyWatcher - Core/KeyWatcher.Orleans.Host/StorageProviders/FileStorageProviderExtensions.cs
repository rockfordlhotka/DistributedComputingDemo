using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;

namespace KeyWatcher.Orleans.Host.StorageProviders
{
	public static class FileStorageProviderExtensions
	{
		public static ISiloHostBuilder AddFileStorage(this ISiloHostBuilder builder, string name, Action<OptionsBuilder<FileStorageProviderOptions>> configureOptions = null) =>
			builder
				.ConfigureApplicationParts(parts => parts.AddFrameworkPart(typeof(FileStorageProvider).Assembly))
				.ConfigureServices(services =>
				{
					configureOptions?.Invoke(services.AddOptions<FileStorageProviderOptions>(name));
					services.ConfigureNamedOptionForLogging<FileStorageProviderOptions>(name);
					services.TryAddSingleton<IGrainStorage>(
						instance => instance.GetServiceByName<IGrainStorage>(name));
					services.AddSingletonNamedService<IGrainStorage>(name, FileStorageProviderFactory.Create);
				});
	}
}
