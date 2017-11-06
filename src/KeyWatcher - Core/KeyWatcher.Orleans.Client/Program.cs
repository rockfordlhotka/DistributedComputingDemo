using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
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

			while (true)
			{
				var buffer = new char[Program.BufferSize];
				await Console.In.ReadAsync(buffer, 0, buffer.Length);
				var content = new string(buffer);

				if (content.Contains("STOP IT")) { break; }

				var user = client.GetGrain<IUserGrain>(userName);
				await user.ProcessAsync(new UserKeysMessage(userName, buffer));
			}
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
					var configuration = ClientConfiguration.LocalhostSilo();
					client = new ClientBuilder()
						.UseConfiguration(configuration)
						.AddApplicationPartsFromReferences(typeof(IUserGrain).Assembly)
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
