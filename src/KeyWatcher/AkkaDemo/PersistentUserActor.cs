using Akka.Persistence;
using System;

namespace AkkaDemo
{
	public sealed class PersistentUserActor
		: ReceivePersistentActor
	{
		private int messageCount = 0;

		public PersistentUserActor(uint id)
		{
			this.Id = id;
			Console.Out.WriteLine($"New user {this.Id}");

			this.Recover<int>(message =>
			{
				Console.Out.WriteLine($"Recover<int>: user {this.Id} - message: {message}");
				this.Handle(message);
			});
			this.Recover<SnapshotOffer>(offer =>
			{
				Console.Out.WriteLine($"Recover<SnapshotOffer>: user {this.Id} - message: {offer.Snapshot}");
				this.Count = (int)offer.Snapshot;
			});

			this.Command<int>(message =>
			{
				this.Handle(message);
				this.Persist(message, _ =>
				{
					if (++this.messageCount % 5 == 0)
					{
						this.SaveSnapshot(this.Count);
						Console.Out.WriteLine($"Snapshot taken: user {this.Id} - count: {this.Count}");
					}
				});
			});
			this.Command<SaveSnapshotSuccess>(success =>
			{
				this.DeleteMessages(success.Metadata.SequenceNr);
			});
		}

		public override string PersistenceId
		{
			get { return this.Id.ToString(); }
		}

		private void Handle(int message)
		{
			if (message == -1)
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
