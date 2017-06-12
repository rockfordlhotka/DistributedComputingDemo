using KeyWatcher.Messages;
using KeyWatcher.SFA.UserActor.Interfaces;
using KeyWatching;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Linq;
using System.Windows.Forms;

namespace KeyWatcher.SFA.Client
{
	class Program
	{
		private const ushort BufferSize = 20;

		static void Main(string[] args)
		{
			Console.Out.WriteLine("Press enter to continue...");
			Console.In.ReadLine();

			var userName = Program.GetUserName();
			var user = ActorProxy.Create<IUserActor>(
				new ActorId(Program.GetUserName()), "fabric:/KeyWatcher.SFA");

			using (var keyLogger = new BufferedEventedNativeKeyWatcher(Program.BufferSize))
			{
				keyLogger.KeysLogged += (s, e) =>
				{
					Console.Out.WriteLine("Got here");
					var keys = e.Keys.ToArray();
					user.ProcessAsync(new UserKeysMutableMessage { Name = userName, Keys = keys });
				};

				Application.Run();
			}
		}

		private static string GetUserName() =>
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";
	}
}
