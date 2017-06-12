using System;
using System.Threading.Tasks;
using KeyWatcher.Dependencies;

namespace KeyWatcher.SFA.UserActor
{
	internal sealed class SFANotification
		: INotification
	{
		public Task SendAsync(string recipient, string title, string message)
		{
			ActorEventSource.Current.Message(
				$"To: {recipient}, Title: {title}, Message: {message}");
			return Task.CompletedTask;
		}
	}
}
