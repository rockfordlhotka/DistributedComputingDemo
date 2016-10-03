using System;
using Akka.Actor;
using KeyWatcher.Dependencies;
using KeyWatcher.Messages;

namespace KeyWatcher.Actors
{
	public sealed class EmailActor
		: ReceiveActor
	{
		private readonly INotification notification;

		public EmailActor(INotification notification)
		{
			if (notification == null)
			{
				throw new ArgumentNullException(nameof(notification));
			}

			this.notification = notification;
			this.Receive<UserBadWordsMessage>(message => this.Handle(message));
		}

		private void Handle(UserBadWordsMessage message)
		{
			this.notification.SendAsync("ITWatchers@YourCompany.com", "BAD WORDS SAID",
				$"The user {message.User} typed the following bad words: {string.Join(", ", message.BadWords)}")
				.PipeTo(this.Self);
		}
	}
}
