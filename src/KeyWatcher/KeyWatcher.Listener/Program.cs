using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using KeyWatcher.Actors;
using KeyWatcher.Dependencies;
using System;

namespace KeyWatcher.Listener
{
	class Program
	{
		static void Main(string[] args)
		{
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
			var builder = new ContainerBuilder();
			builder.RegisterModule<DependenciesModule>();
			builder.RegisterModule<ActorsModule>();
			var container = builder.Build();

			using (var system = ActorSystem.Create("KeyWatcherListener", config))
			{
				new AutoFacDependencyResolver(container, system);

				system.ActorOf(system.DI().Props<UsersActor>(), "users");
				Console.WriteLine("Users actor hosted.");
				Console.ReadLine();
			}
		}
	}
}
