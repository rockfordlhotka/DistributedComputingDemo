using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Autofac.Integration.WebApi;
using KeyWatcher.Actors;
using KeyWatcher.Dependencies;
using Microsoft.AspNet.SignalR;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KeyWatcher.Azure
{
	public class WebApiApplication 
		: HttpApplication
	{
		private ActorSystem system;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			var hub = GlobalHost.ConnectionManager.GetHubContext<KeyWatcherHub>();
			this.system = ActorSystem.Create("KeyWatcherListener");

			var builder = new ContainerBuilder();
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			builder.RegisterModule(new DependenciesModule(hub));
			builder.RegisterModule<ActorsModule>();
			var container = builder.Build();

			new AutoFacDependencyResolver(container, system);
			var usersActor = this.system.ActorOf(system.DI().Props<UsersActor>(), "users");

			var azureBuilder = new ContainerBuilder();
			azureBuilder.RegisterModule(new AzureModule(usersActor));
			azureBuilder.Update(container);

			GlobalConfiguration.Configuration.DependencyResolver =
				new AutofacWebApiDependencyResolver(container);
		}
	}
}
