using Akka.Actor;
using Akka.DI.Core;
using KeyWatcher.Dependencies;
using KeyWatcher.Messages;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace KeyWatcher.Actors
{
	public sealed class UserActor
		: TypedActor, IHandle<UserKeysMessage>
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

		public void Handle(UserKeysMessage message)
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
				email.Tell(new UserBadWordsMessage(message.User, foundBadWords.ToImmutableArray()));
			}
		}

		protected override void PreStart()
		{
			this.logger.LogAsync($"{nameof(UserActor)}.{nameof(this.PreStart)}");
		}

		protected override void PostStop()
		{
			this.logger.LogAsync($"{nameof(UserActor)}.{nameof(this.PostStop)}");
		}
	}
}
