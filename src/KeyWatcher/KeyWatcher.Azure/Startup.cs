using Owin;

namespace KeyWatcher.Azure
{
	public sealed class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}