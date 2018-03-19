using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.Storage;
using System;

namespace KeyWatcher.Orleans.Host.StorageProviders
{
	internal static class FileStorageProviderFactory
	{
		internal static IGrainStorage Create(IServiceProvider services, string name)
		{
			var optionsSnapshot = services.GetRequiredService<IOptionsSnapshot<FileStorageProviderOptions>>();
			return ActivatorUtilities.CreateInstance<FileStorageProvider>(services, optionsSnapshot.Get(name), name);
		}
	}
}
