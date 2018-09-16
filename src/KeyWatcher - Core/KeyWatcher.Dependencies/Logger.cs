using System;
using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	internal sealed class Logger
		: ILogger
	{
		public Task LogAsync(string message) => 
			Console.Out.WriteLineAsync($"Log: {message}");
	}
}
