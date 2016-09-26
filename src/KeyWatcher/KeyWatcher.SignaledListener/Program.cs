using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using KeyWatcher.Actors;
using KeyWatcher.Dependencies;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;

namespace KeyWatcher.SignaledListener
{
	class Program
	{
		public const string SignalRHubUrl = "http://localhost:5944";

		static void Main(string[] args)
		{
			using (WebApp.Start(Program.SignalRHubUrl))
			{
				var hub = GlobalHost.ConnectionManager.GetHubContext<KeyWatcherHub>();

				var builder = new ContainerBuilder();
				builder.RegisterModule(new DependenciesModule(hub));
				builder.RegisterModule<ActorsModule>();
				var container = builder.Build();

				var config = ConfigurationFactory.ParseString(@"
akka {
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }

    remote {
        helios.tcp {
            port = 4545
            hostname = localhost
        }
    }
}");

				using (var system = ActorSystem.Create("KeyWatcherListener", config))
				{
					new AutoFacDependencyResolver(container, system);

					system.ActorOf(system.DI().Props<UsersActor>(), "users");
					Console.WriteLine("Users actor hosted.");
					Console.ReadKey();
				}
			}
		}
	}
}
