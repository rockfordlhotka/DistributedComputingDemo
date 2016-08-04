using Akka.Actor;
using Akka.Configuration;
using KeyWatcher.Actors;
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
            port = 8080
            hostname = localhost
        }
    }
}");

			using (var userSystem = ActorSystem.Create("UserSystem", config))
			{
				userSystem.ActorOf<UserActor>("user");
				Console.WriteLine("User actor hosted.");
				Console.ReadKey();
			}
		}
	}
}
