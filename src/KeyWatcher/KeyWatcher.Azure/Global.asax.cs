using KeyWatcher.Orleans.Grains;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Orleans.Storage;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SWH = System.Web.Http;

namespace KeyWatcher.Azure
{
	public class WebApiApplication 
		: HttpApplication
	{
		private SiloHost siloHost;

		protected void Application_Start()
		{
			// TODO: I don't know WHY Orleans doesn't work if I don't do this...
			// But this "helps" get Orleans to find the grains.
			var x = typeof(UserGrain);
			AreaRegistration.RegisterAllAreas();
			SWH.GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			// Note: Need Orleans 1.5.0 or higher to do this...
			var configuration = ClusterConfiguration.LocalhostPrimarySilo(11111, 30000);
			configuration.UseStartupType<Startup>();
			configuration.Globals.RegisterStorageProvider<MemoryStorage>("Default");

			this.siloHost = new SiloHost("KeyWatcherSilo", configuration);
			this.siloHost.InitializeOrleansSilo();

			if (!this.siloHost.StartOrleansSilo())
			{
				throw new SystemException($"Failed to start Orleans silo '{this.siloHost.Name}'");
			}

			WebApiApplication.InitializeGrainClient();
		}

		private static void InitializeGrainClient()
		{
			var config = ClientConfiguration.LocalhostSilo(30000);

			while (true)
			{
				try
				{
					var client =
						 new ClientBuilder().UseConfiguration(config).Build();
					client.Connect().GetAwaiter().GetResult();
					break;
				}

				catch
				{
					Task.Delay(TimeSpan.FromSeconds(1));
				}
			}

			GrainClient.Initialize(config);
		}

		protected void Application_End()
		{
			if (this.siloHost != null)
			{
				this.siloHost.Dispose();
				GC.SuppressFinalize(this.siloHost);
				this.siloHost = null;
			}
		}
	}
}
