namespace KeyWatcher.Actors.Messages
{
	public sealed class UserKeys
	{
		public UserKeys(string user, char[] keys)
		{
			this.User = user;
			this.Keys = keys;
		}

		public string User { get; }
		public char[] Keys { get; }
	}
}
