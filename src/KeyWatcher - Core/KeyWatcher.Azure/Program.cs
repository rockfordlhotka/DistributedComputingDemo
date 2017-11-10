using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace KeyWatcher.Azure
{
	public class Program
	{
		public static void Main(string[] args) =>
			Program.BuildWebHost(args).Run();

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
