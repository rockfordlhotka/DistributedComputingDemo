using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using Orleans.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Client
{
	public static class Program
	{
		private const int BufferSize = 20;
		private static readonly TimeSpan Retry = TimeSpan.FromSeconds(1);
		private const int RetryAttempts = 5;

		public static async Task Main(string[] args)
		{
			var userName = Program.GetUserName();
			var client = await Program.StartClientWithRetries();
			await Console.Out.WriteLineAsync("Begin...");

			var keyLogger = new BufferedEventedKeyWatcher(Program.BufferSize);
			keyLogger.KeysLogged += async (s, e) => 
			{
				var keyBuffer = e.Keys.ToArray();
				var content = new string(keyBuffer);

				if (content.Contains("STOP IT")) { keyLogger.Cancel(); }
				else
				{
					var user = client.GetGrain<IUserGrain>(userName);
					await user.ProcessAsync(new UserKeysMessage(userName, keyBuffer));
				}
			};

			keyLogger.Listen();
		}

		private static string GetUserName() =>
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";

		private static async Task<IClusterClient> StartClientWithRetries(
			int initializeAttemptsBeforeFailing = Program.RetryAttempts)
		{
			var attempt = 0;
			IClusterClient client;

			while (true)
			{
				try
				{
					client = new ClientBuilder()
						.UseLocalhostClustering()
						.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUserGrain).Assembly).WithReferences())
						//.ConfigureLogging(logging => logging.AddConsole())
						.Build();

					await client.Connect();
					Console.WriteLine("Client successfully connect to silo host");
					break;
				}
				catch (SiloUnavailableException)
				{
					attempt++;

					Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");

					if (attempt > initializeAttemptsBeforeFailing)
					{
						throw;
					}

					await Task.Delay(Program.Retry);
				}
			}

			return client;
		}
	}
}
