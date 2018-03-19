namespace KeyWatcher.Messages
{
	public sealed class NotificationMessage
	{
		public NotificationMessage(string recipient,
			string title, string message)
		{
			this.Recipient = recipient;
			this.Title = title;
			this.Message = message;
		}

		public string Recipient { get; }
		public string Title { get; }
		public string Message { get; }
	}
}
