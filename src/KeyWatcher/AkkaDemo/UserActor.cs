using Akka.Actor;
using System;

namespace AkkaDemo
{
	public sealed class UserActor
		: ReceiveActor
	{
		public UserActor(uint id)
		{
			this.Id = id;
			Console.Out.WriteLine($"New user {this.Id}");
			this.Receive<int>(message => this.Handle(message));
		}

		private void Handle(int message)
		{
			if(message == -1)
			{
				throw new NotSupportedException("A value of -1 is not supported.");
			}

			this.Count += message;
			Console.Out.WriteLine($"User {this.Id} - current count: {this.Count}");
		}
		
		public int Count { get; private set; }
		public uint Id { get; }
	}
}
