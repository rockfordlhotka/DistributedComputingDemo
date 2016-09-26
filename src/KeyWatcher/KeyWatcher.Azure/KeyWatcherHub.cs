using Microsoft.AspNet.SignalR;
using System;

namespace KeyWatcher.Azure
{
	public sealed class KeyWatcherHub
		: Hub
	{
		public void Observed(string approach)
		{
			Console.Out.WriteLine($"Observed through {approach}");
		}
	}
}