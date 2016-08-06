using Microsoft.Owin.Cors;
using Owin;

namespace KeyWatcher.SignaledListener
{
	public sealed class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
		}
	}
}
