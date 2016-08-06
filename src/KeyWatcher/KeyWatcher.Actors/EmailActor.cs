using System;
using Akka.Actor;
using KeyWatcher.Actors.Messages;
using KeyWatcher.Dependencies;

namespace KeyWatcher.Actors
{
	public sealed class EmailActor
		: TypedActor, IHandle<UserBadWords>
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

		public void Handle(UserBadWords message)
		{
			this.email.SendAsync("ITWatchers@YourCompany.com", "BAD WORDS SAID",
				$"The user {message.User} typed the following bad words: {string.Join(", ", message.BadWords)}")
				.PipeTo(this.Self);
		}
	}
}
