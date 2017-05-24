using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using Newtonsoft.Json;

namespace KeyWatcher.Orleans.Host.StorageProviders
{
	public sealed class FileStorageProvider
		: IStorageProvider
	{
		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) =>
			throw new NotImplementedException();

		public Task Close() =>
			throw new NotImplementedException();

		public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
		{
			this.Name = name;
			if (string.IsNullOrWhiteSpace(config.Properties["RootDirectory"]))
			{
				throw new ArgumentException("RootDirectory property not set");
			}

			var directory = new System.IO.DirectoryInfo(config.Properties["RootDirectory"]);
			if (!directory.Exists)
			{
				directory.Create();
			}

			this.RootDirectory = directory.FullName;

			return TaskDone.Done;
		}

		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			var collectionName = grainState.GetType().Name;
			var key = grainReference.ToKeyString();

			var fName = key + "." + collectionName;
			var path = System.IO.Path.Combine(this.RootDirectory, fName);

			var fileInfo = new System.IO.FileInfo(path);
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
			var storedData = JsonConvert.SerializeObject(grainState.State);

			var collectionName = grainState.GetType().Name;
			var key = grainReference.ToKeyString();

			var fName = key + "." + collectionName;
			var path = System.IO.Path.Combine(this.RootDirectory, fName);

			var fileInfo = new System.IO.FileInfo(path);

			using (var stream = new System.IO.StreamWriter(
						  fileInfo.Open(System.IO.FileMode.Create,
											 System.IO.FileAccess.Write)))
			{
				await stream.WriteAsync(storedData);
			}
		}

		public Logger Log { get; set; }
		public string Name { get; set; }
		public string RootDirectory { get; private set; }
	}
}
