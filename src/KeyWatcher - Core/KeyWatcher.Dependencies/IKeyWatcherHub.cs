using KeyWatcher.Messages;

namespace KeyWatcher.Dependencies
{
	public interface IKeyWatcherHub
	{
		void NotificationSent(NotificationMessage message);
	}
}
