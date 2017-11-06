namespace KeyWatcher.Messages
{
	public sealed class UserBadWordsMessage
	{
		public UserBadWordsMessage(string user, string[] badWords)
		{
			this.User = user;
			this.BadWords = badWords;
		}

		public string[] BadWords { get; }
		public string User { get; }
	}
}
