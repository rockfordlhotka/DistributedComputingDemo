using Autofac;
using Autofac.Integration.WebApi;
using KeyWatcher.Dependencies;
using Microsoft.AspNet.SignalR;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace KeyWatcher.Azure
{
	public class WebApiApplication 
		: HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			var hub = GlobalHost.ConnectionManager.GetHubContext<KeyWatcherHub>();

			// TODO: Host Orleans here...
			var builder = new ContainerBuilder();
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			builder.RegisterModule(new DependenciesModule(hub));
			var container = builder.Build();

			var azureBuilder = new ContainerBuilder();

			GlobalConfiguration.Configuration.DependencyResolver =
				new AutofacWebApiDependencyResolver(container);
		}
	}
}
