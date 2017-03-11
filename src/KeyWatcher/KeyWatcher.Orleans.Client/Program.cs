using KeyWatcher.Messages;
using KeyWatcher.Orleans.Contracts;
using Orleans;
using Orleans.Runtime.Configuration;
using System;

namespace KeyWatcher.Orleans.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Out.WriteLine("Waiting for Orleans Silo to start. Press Enter to proceed...");
			Console.In.ReadLine();

			var config = ClientConfiguration.LocalhostSilo(30000);
			GrainClient.Initialize(config);

			var user = GrainClient.GrainFactory.GetGrain<IUserGrain>("a");
			user.Process(new UserKeysMessage("a", new[] { 'a', 'c', 'o', 't', 't', 'o', 'n', 'b' }));
		}
	}
}
