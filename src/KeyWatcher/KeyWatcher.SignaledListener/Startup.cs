using Microsoft.Owin.Cors;
using Owin;

namespace KeyWatcher.SignaledListener
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
		}
	}
}
