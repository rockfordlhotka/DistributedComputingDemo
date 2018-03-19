using KeyWatcher.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Polly;
using System;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using static KeyWatcher.Signaled.Client.Extensions.HubConnectionExtensions;

namespace KeyWatcher.Signaled.Client
{
	class Program
	{
		private const int BufferSize = 20;
		private const int RetryCount = 4;
		private static readonly TimeSpan Retry = TimeSpan.FromMilliseconds(100);

		static async Task Main(string[] args)
		{
			await Console.Out.WriteLineAsync("Setting up SignalR client...");
			var connection = new HubConnectionBuilder()
				.WithUrl(Common.KeyWatcherHubApiUri)
				.WithConsoleLogger()
				.Build();

			connection.On<NotificationMessage>(Common.NotificationSent, data =>
			{
				Console.WriteLine();
				Console.WriteLine($"{nameof(HubConnectionExtensions.On)} {Common.NotificationSent} received: {data.Message}");
				Console.WriteLine();
			});
			connection.Observe<NotificationMessage>(Common.NotificationSent)
				.Where(message => message.Message.Contains("cotton"))
				.Take(3)
				.Delay(TimeSpan.FromSeconds(2))
				.Subscribe(message =>
				{
					Console.WriteLine();
					Console.WriteLine(
						$"{nameof(KeyWatcher.Signaled.Client.Extensions.HubConnectionExtensions.Observe)} {Common.NotificationSent} received: {message.Message}");
					Console.WriteLine();
				});

			await connection.StartAsync();

			await Console.Out.WriteLineAsync("Setting up HTTP call policy...");
			var userName = Common.GetUserName();
			var client = new HttpClient();
			var httpPolicy = Policy.Handle<HttpRequestException>(e =>
				{
					Console.WriteLine($"Could not call the service, sorry!");
					return true;
				}).WaitAndRetryAsync(
				Program.RetryCount, retryAttempt =>
				{
					Console.WriteLine($"Attempt {retryAttempt} of {Program.RetryCount} to call service...");
					return Program.Retry;
				});

			await Console.Out.WriteLineAsync("Setting up key watcher...");
			var keyLogger = new BufferedEventedKeyWatcher(Program.BufferSize);
			keyLogger.KeysLogged += async (s, e) =>
			{
				var message = JsonConvert.SerializeObject(
					new UserKeysMessage(userName, e.Keys.ToArray()), Formatting.Indented);
				var content = new StringContent(message,
					Encoding.Unicode, "application/json");

				await httpPolicy.ExecuteAsync(async () => await client.PostAsync(Common.KeyWatcherApiUri, content));
			};


			await Console.Out.WriteLineAsync("Begin signaled client.");
			keyLogger.Listen();
		}
	}
}
