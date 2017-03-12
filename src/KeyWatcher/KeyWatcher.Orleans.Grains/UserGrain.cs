using KeyWatcher.Dependencies;
using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class UserGrain
		: Grain<UserGrainState>, IUserGrain
	{
		private static readonly string[] BadWords = { "cotton", "headed", "ninny", "muggins" };
		private readonly ILogger logger;
		private readonly Lazy<INotification> notification;

		public UserGrain(ILogger logger, Lazy<INotification> notification)
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

		public override async Task OnActivateAsync()
		{
			this.Id = this.GetPrimaryKeyString();
			await this.logger.LogAsync($"{nameof(OnActivateAsync)} - user ID is {this.Id}");
			await base.OnActivateAsync();
		}

		public override async Task OnDeactivateAsync()
		{
			await this.logger.LogAsync($"{nameof(OnDeactivateAsync)} - user ID is {this.Id}");
			await base.OnDeactivateAsync();
		}

		public string Id { get; private set; }

		public async Task ProcessAsync(UserKeysMessage message)
		{
			var keys = new string(message.Keys).ToLower();

			await this.logger.LogAsync($"Received message from {message.Name}: {keys}");

			if (keys.Contains("die"))
			{
				throw new NotSupportedException("I will never die!");
			}

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
				this.State.BadWords.AddRange(foundBadWords);
				await this.WriteStateAsync();

				var badWords = string.Join(", ", foundBadWords);
				await this.notification.Value.SendAsync("ITWatchers@YourCompany.com", "BAD WORDS SAID",
					$"The user {message.Name} typed the following bad words: {badWords}");
			}

			await this.logger.LogAsync($"Bad word count for {message.Name}: {this.State.BadWords.Count}");
		}
	}
}
