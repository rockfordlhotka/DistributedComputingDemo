namespace AkkaDemo
{
	public sealed class UserCountMessage
	{
		public UserCountMessage(uint id, int count)
		{
			this.Id = id;
			this.Count = count;
		}

		public uint Id { get; }
		public int Count { get; }
	}
}
