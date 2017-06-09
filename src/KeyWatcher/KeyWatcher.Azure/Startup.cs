using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using System;
using System.Web.Http;

namespace KeyWatcher.Azure
{
	public sealed class Startup
	{
		public void Configuration(IAppBuilder app) =>
			app.MapSignalR();

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			var hub = GlobalHost.ConnectionManager.GetHubContext<KeyWatcherHub>();

			var builder = new ContainerBuilder();
			builder.RegisterModule(new AzureModule(hub, services));
			var container = builder.Build();

			GlobalConfiguration.Configuration.DependencyResolver =
				new AutofacWebApiDependencyResolver(container);
			return new AutofacServiceProvider(container);
		}
	}
}