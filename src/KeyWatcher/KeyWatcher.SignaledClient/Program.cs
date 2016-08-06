using Microsoft.AspNet.SignalR.Client;
using System;

namespace KeyWatcher.SignaledClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var connection = new HubConnection("http://localhost:8181");
			var proxy = connection.CreateHubProxy("KeyWatcherHub");

			proxy.On("NotificationSent",
				message => { Console.Out.WriteLine(message); });
			Console.ReadLine();
		}
	}
}
