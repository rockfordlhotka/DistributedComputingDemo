using System.Collections.Immutable;

namespace KeyWatcher.Actors.Messages
{
	public sealed class UserBadWords
	{
		public UserBadWords(string user, ImmutableArray<string> badWords)
		{
			this.User = user;
			this.BadWords = badWords;
		}

		public ImmutableArray<string> BadWords { get; }
		public string User { get; }
	}
}
