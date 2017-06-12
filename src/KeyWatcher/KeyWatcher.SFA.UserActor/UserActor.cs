using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using KeyWatcher.SFA.UserActor.Interfaces;
using KeyWatcher.Messages;
using System;
using KeyWatcher.Dependencies;

namespace KeyWatcher.SFA.UserActor
{
	[StatePersistence(StatePersistence.Persisted)]
	internal class UserActor
		: Actor, IUserActor
	{
		private static readonly string[] BadWords = { "cotton", "headed", "ninny", "muggins" };
		private UserActorState state;
		private readonly Lazy<INotification> notification;

		public UserActor(ActorService actorService, ActorId actorId, Lazy<INotification> notification)
			 : base(actorService, actorId) =>
			this.notification = notification;

		public async Task ProcessAsync(UserKeysMutableMessage message)
		{
			var keys = new string(message.Keys.ToArray()).ToLower();

			var foundBadWords = new List<string>();

			foreach (var word in UserActor.BadWords)
			{
				if (keys.Contains(word))
				{
					foundBadWords.Add(word);
				}
			}

			if (foundBadWords.Count > 0)
			{
				this.state.BadWords.AddRange(foundBadWords);
				await this.StateManager.AddOrUpdateStateAsync(this.Id.ToString(), this.state, (id, state) => this.state);
				await this.StateManager.SaveStateAsync();

				var badWords = string.Join(", ", foundBadWords);
				await this.notification.Value.SendAsync("ITWatchers@YourCompany.com", "BAD WORDS SAID",
					$"The user {message.Name} typed the following bad words: {badWords}");
			}

			ActorEventSource.Current.ActorMessage(
				this, $"Bad word count for {message.Name}: {this.state.BadWords.Count}");
		}

		protected override async Task OnActivateAsync()
		{
			var id = this.Id.ToString();
			var getState = await this.StateManager.TryGetStateAsync<UserActorState>(id);

			if (getState.HasValue)
			{
				this.state = getState.Value;
			}
			else
			{
				this.state = new UserActorState();
			}
		}
	}
}
