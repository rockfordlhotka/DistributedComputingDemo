namespace KeyWatcher.Messages
{
	public sealed class UserKeysMessage
	{
		public UserKeysMessage(string user, char[] keys)
		{
			this.User = user;
			this.Keys = keys;
		}

		public string User { get; }
		public char[] Keys { get; }
	}
}
