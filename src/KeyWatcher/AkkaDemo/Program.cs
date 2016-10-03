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
				Console.Out.WriteLine($"Users actor at {usersActor.Path.ToStringWithAddress()}");
				Program.CreateUsers(usersActor);
				//Program.CreateUsersWithNonPersistableActor(usersActor);
				//Program.CreateUsersWithPersistableActor(usersActor);
				//Program.CreateUsersWithNonPersistableActorAndException(usersActor);
				//Program.CreateUsersWithPersistableActorAndException(usersActor);
				Console.ReadLine();
			}
		}

		private static void CreateUsers(IActorRef usersActor)
		{
			usersActor.Tell(new UserCountMessage(1, 3));
			usersActor.Tell(new UserCountMessage(2, 5));
			usersActor.Tell(new UserCountMessage(3, 10));
			usersActor.Tell(new UserCountMessage(1, 4));
			usersActor.Tell(new UserCountMessage(4, 1));
			usersActor.Tell(new UserCountMessage(2, 2));
			usersActor.Tell(new UserCountMessage(3, 2));
		}

		private static void CreateUsersWithNonPersistableActor(IActorRef usersActor)
		{
			usersActor.Tell(new UserCountMessage(2, 1));
			usersActor.Tell(new UserCountMessage(2, 2));
			usersActor.Tell(new UserCountMessage(2, 3));
			usersActor.Tell(new UserCountMessage(2, 4));
			usersActor.Tell(new UserCountMessage(2, 5));
			usersActor.Tell(new UserCountMessage(2, 6));
			usersActor.Tell(new UserCountMessage(2, 7));
		}

		private static void CreateUsersWithNonPersistableActorAndException(IActorRef usersActor)
		{
			usersActor.Tell(new UserCountMessage(2, 1));
			usersActor.Tell(new UserCountMessage(2, 2));
			usersActor.Tell(new UserCountMessage(2, 3));
			usersActor.Tell(new UserCountMessage(2, 4));
			usersActor.Tell(new UserCountMessage(2, 5));
			usersActor.Tell(new UserCountMessage(2, 6));
			usersActor.Tell(new UserCountMessage(2, -1));
			usersActor.Tell(new UserCountMessage(2, 7));
		}

		private static void CreateUsersWithPersistableActor(IActorRef usersActor)
		{
			usersActor.Tell(new UserCountMessage(3, 1));
			usersActor.Tell(new UserCountMessage(3, 2));
			usersActor.Tell(new UserCountMessage(3, 3));
			usersActor.Tell(new UserCountMessage(3, 4));
			usersActor.Tell(new UserCountMessage(3, 5));
			usersActor.Tell(new UserCountMessage(3, 6));
			usersActor.Tell(new UserCountMessage(3, 7));
		}

		private static void CreateUsersWithPersistableActorAndException(IActorRef usersActor)
		{
			usersActor.Tell(new UserCountMessage(3, 1));
			usersActor.Tell(new UserCountMessage(3, 2));
			usersActor.Tell(new UserCountMessage(3, 3));
			usersActor.Tell(new UserCountMessage(3, 4));
			usersActor.Tell(new UserCountMessage(3, 5));
			usersActor.Tell(new UserCountMessage(3, 6));
			usersActor.Tell(new UserCountMessage(3, -1));
			usersActor.Tell(new UserCountMessage(3, 7));
			usersActor.Tell(new UserCountMessage(3, -1));
			usersActor.Tell(new UserCountMessage(3, 8));
		}
	}
}
