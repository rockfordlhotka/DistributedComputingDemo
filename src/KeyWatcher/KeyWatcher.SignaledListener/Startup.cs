using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System.Reflection;

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
