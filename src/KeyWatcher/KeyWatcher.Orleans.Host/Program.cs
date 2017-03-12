using Orleans.Runtime.Host;
using System;
using System.Net;

namespace KeyWatcher.Orleans.Host
{
	class Program
	{
		private static SiloHost siloHost;

		static void Main(string[] args)
		{
			var hostDomain = AppDomain.CreateDomain("OrleansHost", null,
				 new AppDomainSetup()
				 {
					 AppDomainInitializer = Program.InitializeSilo
				 });

			Console.WriteLine("Orleans silo is running.");
			Console.WriteLine("Press Enter to terminate...");
			Console.ReadLine();

			hostDomain.DoCallBack(Program.ShutdownSilo);
		}

		private static void InitializeSilo(string[] args)
		{
			Program.siloHost = new SiloHost(Dns.GetHostName());
			Program.siloHost.ConfigFileName = "OrleansConfiguration.xml";

			Program.siloHost.InitializeOrleansSilo();

			if (!siloHost.StartOrleansSilo())
			{
				throw new SystemException(
					$"Failed to start Orleans silo '{siloHost.Name}' as a {siloHost.Type} node");
			}
		}

		private static void ShutdownSilo()
		{
			if (Program.siloHost != null)
			{
				siloHost.Dispose();
				GC.SuppressFinalize(siloHost);
				siloHost = null;
			}
		}
	}
}
