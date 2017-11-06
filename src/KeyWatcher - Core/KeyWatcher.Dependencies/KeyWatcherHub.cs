using Microsoft.AspNetCore.SignalR;

namespace KeyWatcher.Dependencies
{
	public sealed class KeyWatcherHub
		: Hub<IKeyWatcherHub>
	{ }
}
