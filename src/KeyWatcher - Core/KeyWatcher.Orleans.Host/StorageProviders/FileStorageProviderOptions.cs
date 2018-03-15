namespace KeyWatcher.Orleans.Host.StorageProviders
{
	public sealed class FileStorageProviderOptions
	{
		public FileStorageProviderOptions() => this.RootDirectory = @".\Storage";

		public string RootDirectory { get; set; }
	}
}
