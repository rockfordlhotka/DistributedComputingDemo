using Akka.Actor;
using Akka.DI.Core;
using KeyWatcher.Actors.Messages;
using KeyWatcher.Dependencies;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace KeyWatcher.Actors
{
	public sealed class UserActor
		: TypedActor, IHandle<UserKeys>
	{
		private static readonly string[] BadWords = { "cotton", "headed", "ninny", "muggins" };
		private readonly ILogger logger;

		public UserActor(ILogger logger)
		{
			if (logger == null)
			{
				throw new ArgumentNullException(nameof(logger));
			}

			this.logger = logger;
		}

		public void Handle(UserKeys message)
		{
			var keys = new string(message.Keys).ToLower();
			this.logger.LogAsync($"Received message from {message.User}: {keys}")
				.PipeTo(this.Self);

			var foundBadWords = new List<string>();

			foreach(var word in UserActor.BadWords)
			{
				if(keys.Contains(word))
				{
					foundBadWords.Add(word);
				}
			}

			if(foundBadWords.Count > 0)
			{
				var email = Context.ActorOf(Context.DI().Props<EmailActor>());
				email.Tell(new UserBadWords(message.User, foundBadWords.ToImmutableArray()));
			}
		}
	}
}
