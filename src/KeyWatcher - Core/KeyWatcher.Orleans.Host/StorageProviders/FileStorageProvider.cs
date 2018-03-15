using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Runtime;
using Orleans.Storage;
using Newtonsoft.Json;
using System.IO;

namespace KeyWatcher.Orleans.Host.StorageProviders
{
	public sealed class FileStorageProvider
		: IGrainStorage
	{
		public FileStorageProvider(string name, FileStorageProviderOptions options)
		{
			this.Name = name;
			var rootDirectory = options.RootDirectory;

			if (string.IsNullOrWhiteSpace(rootDirectory))
			{
				throw new ArgumentException("RootDirectory property not set");
			}

			var directory = new DirectoryInfo(rootDirectory);

			if (!directory.Exists)
			{
				directory.Create();
			}

			this.RootDirectory = directory.FullName;
		}

		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) =>
			throw new NotImplementedException();

		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			var fileInfo = this.GetFileInfo(grainReference, grainState);

			if (fileInfo.Exists)
			{
				using (var stream = fileInfo.OpenText())
				{
					var storedData = await stream.ReadToEndAsync();
					grainState.State = JsonConvert.DeserializeObject(
						storedData, grainState.State.GetType());
				}
			}
		}

		public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			var fileInfo = this.GetFileInfo(grainReference, grainState);

			using (var stream = new StreamWriter(
				fileInfo.Open(FileMode.Create, FileAccess.Write)))
			{
				await stream.WriteAsync(
					JsonConvert.SerializeObject(grainState.State));
			}
		}

		private FileInfo GetFileInfo(GrainReference grainReference, IGrainState grainState)
		{
			var collectionName = grainState.GetType().Name;
			var key = grainReference.ToKeyString();
			var path = Path.Combine(this.RootDirectory, $"{key}.{collectionName}");

			return new FileInfo(path);
		}

		public string Name { get; }
		public string RootDirectory { get; }
	}
}
