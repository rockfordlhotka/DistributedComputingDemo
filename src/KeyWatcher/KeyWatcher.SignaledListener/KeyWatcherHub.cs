using Microsoft.AspNet.SignalR;
using System;

namespace KeyWatcher.SignaledListener
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
