using Akka.Actor;
using System;

namespace AkkaDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var system = ActorSystem.Create("UserSystem"))
			{
				var usersActor = system.ActorOf<UsersActor>("users");
				//usersActor.Tell(new UserCountMessage(1, 3));
				usersActor.Tell(new UserCountMessage(2, 5));
				//usersActor.Tell(new UserCountMessage(3, 10));
				//usersActor.Tell(new UserCountMessage(1, 4));
				//usersActor.Tell(new UserCountMessage(4, 1));
				//usersActor.Tell(new UserCountMessage(3, -2));
				usersActor.Tell(new UserCountMessage(2, -1));
				usersActor.Tell(new UserCountMessage(2, 2));

				Console.ReadLine();
			}
		}
	}
}
