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
using System.Collections.Generic;
using System.Linq;

namespace KeyWatcher.Orleans.Client
{
	class Program
	{
		private const ushort BufferSize = 20;
		private const string Termination = "STOP IT";
		private static string userName;

		private const string LocalUri = "http://localhost:6344/api/keywatcher";
		private const string AzureUri = "http://keywatcher.azurewebsites.net/api/keywatcher";

		static void Main(string[] args) =>
			Program.UseOrleansLocally();
		//Program.UseOrleansViaWebApiOnAzure(Program.AzureUri);

		private static void UseOrleansLocally()
		{
			Program.userName = Program.GetUserName();

			Console.Out.WriteLine("Waiting for Orleans Silo to start. Press Enter to proceed...");
			Console.In.ReadLine();

			var config = ClientConfiguration.LocalhostSilo(30000);
			GrainClient.Initialize(config);

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

			if(message.Contains(Program.Termination))
			{
				Application.Exit();
			}
		}

		private static void UseOrleansViaWebApi(string url)
		{
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