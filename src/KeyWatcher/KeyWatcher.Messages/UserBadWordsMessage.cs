using System.Collections.Immutable;

namespace KeyWatcher.Messages
{
	public sealed class UserBadWordsMessage
	{
		public UserBadWordsMessage(string user, ImmutableArray<string> badWords)
		{
			this.User = user;
			this.BadWords = badWords;
		}

		public ImmutableArray<string> BadWords { get; }
		public string User { get; }
	}
}
