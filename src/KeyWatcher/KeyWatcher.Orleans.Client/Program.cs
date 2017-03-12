using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using Orleans.Runtime.Configuration;
using System;
using System.Windows.Forms;

namespace KeyWatcher.Orleans.Client
{
	class Program
	{
		private const ushort BufferSize = 20;

		static void Main(string[] args)
		{
			var userName = Program.GetUserName();

			Console.Out.WriteLine("Waiting for Orleans Silo to start. Press Enter to proceed...");
			Console.In.ReadLine();

			var config = ClientConfiguration.LocalhostSilo(30000);
			GrainClient.Initialize(config);

			using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
			{
				keyLogger.KeysLogged += (s, e) =>
				{
					var user = GrainClient.GrainFactory.GetGrain<IUserGrain>(userName);
					user.ProcessAsync(new UserKeysMessage(userName, e.Keys));
				};

				Application.Run();
			}
		}

		private static string GetUserName() => 
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";
	}
}
