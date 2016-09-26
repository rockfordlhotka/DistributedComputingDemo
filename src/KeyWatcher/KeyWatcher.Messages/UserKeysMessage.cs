namespace KeyWatcher.Messages
{
	public sealed class UserKeysMessage
	{
		public UserKeysMessage(string name, char[] keys)
		{
			this.Name = name;
			this.Keys = keys;
		}

		public string Name { get; }
		public char[] Keys { get; }
	}
}
