using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace KeyWatcher.Azure
{
	public static class Program
	{
		public static void Main(string[] args) =>
			Program.BuildWebHost(args).Run();

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureServices(services => services.AddAutofac())
				.UseStartup<Startup>()
				.Build();
	}
}
