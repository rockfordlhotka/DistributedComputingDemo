namespace KeyWatcher.Dependencies
{
	public sealed class SignalRNotificationMessage
	{
		public SignalRNotificationMessage(string recipient,
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
