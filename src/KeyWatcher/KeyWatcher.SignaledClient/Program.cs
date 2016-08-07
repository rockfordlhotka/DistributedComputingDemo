using KeyWatcher.Messages;
using Microsoft.AspNet.SignalR.Client;
using Nito.AsyncEx;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace KeyWatcher.SignaledClient
{
	class Program
	{
		static void Main(string[] args)
		{
			AsyncContext.Run(async () =>
			{
				await Program.MainAsync(args);
			});
		}

		private static async Task MainAsync(string[] args)
		{
			var connection = new HubConnection("http://localhost:5944");
			var proxy = connection.CreateHubProxy("KeyWatcherHub");
			await connection.Start();

			proxy.On<SignalRNotificationMessage>("NotificationSent",
				message => Program.Write(message));

			await Console.In.ReadLineAsync();

			var observable = from item in proxy.Observe("NotificationSent")
								  let m = item[0].ToObject<SignalRNotificationMessage>()
								  select m;

			using (var subscription = observable.Subscribe(
				message => { Console.Out.WriteLine(message); }))
			{
				await Console.In.ReadLineAsync();
			}
		}

		private static void Write(SignalRNotificationMessage message)
		{
			Console.Out.WriteLine(message.Message);
		}
	}
}
