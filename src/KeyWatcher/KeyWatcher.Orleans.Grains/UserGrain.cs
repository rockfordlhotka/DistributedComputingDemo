using KeyWatcher.Dependencies;
using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Grains
{
	public class UserGrain
		: Grain, IUserGrain
	{
		private static readonly string[] BadWords = { "cotton", "headed", "ninny", "muggins" };
		private readonly ILogger logger;
		private readonly INotification notification;

		public UserGrain(ILogger logger, INotification notification)
		{
			if (logger == null)
			{
				throw new ArgumentNullException(nameof(logger));
			}

			if (notification == null)
			{
				throw new ArgumentNullException(nameof(notification));
			}

			this.logger = logger;
			this.notification = notification;
		}

		public async Task Process(UserKeysMessage message)
		{
			var keys = new string(message.Keys).ToLower();

			await this.logger.LogAsync($"Received message from {message.Name}: {keys}");

			var foundBadWords = new List<string>();

			foreach (var word in UserGrain.BadWords)
			{
				if (keys.Contains(word))
				{
					foundBadWords.Add(word);
				}
			}

			if (foundBadWords.Count > 0)
			{
				var badWords = string.Join(", ", foundBadWords);
				await this.notification.SendAsync("ITWatchers@YourCompany.com", "BAD WORDS SAID",
					$"The user {message.Name} typed the following bad words: {badWords}");
			}
		}
	}
}
