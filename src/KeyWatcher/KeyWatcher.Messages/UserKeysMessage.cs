namespace KeyWatcher.Messages
{
	public sealed class UserKeysMessage
	{
		public UserKeysMessage(string user, char[] keys)
		{
			this.Name = user;
			this.Keys = keys;
		}

		public string Name { get; }
		public char[] Keys { get; }
	}
}
