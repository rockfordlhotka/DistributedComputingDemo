using System;
using Akka.Actor;
using KeyWatcher.Dependencies;
using KeyWatcher.Messages;

namespace KeyWatcher.Actors
{
	public sealed class EmailActor
		: TypedActor, IHandle<UserBadWordsMessage>
	{
		private readonly INotification email;

		public EmailActor(INotification email)
		{
			if (email == null)
			{
				throw new ArgumentNullException(nameof(email));
			}

			this.email = email;
		}

		public void Handle(UserBadWordsMessage message)
		{
			this.email.SendAsync("ITWatchers@YourCompany.com", "BAD WORDS SAID",
				$"The user {message.User} typed the following bad words: {string.Join(", ", message.BadWords)}")
				.PipeTo(this.Self);
		}
	}
}
