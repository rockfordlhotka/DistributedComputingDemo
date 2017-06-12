using System;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Runtime;
using KeyWatcher.Dependencies;

namespace KeyWatcher.SFA.UserActor
{
	internal static class Program
	{
		private static void Main()
		{
			try
			{
				ActorRuntime.RegisterActorAsync<UserActor>(
					(context, actorType) => 
						new ActorService(context, actorType, 
							(service, id) => new UserActor(service, id, new Lazy<INotification>(() => new SFANotification()))))
					.GetAwaiter().GetResult();

				Thread.Sleep(Timeout.Infinite);
			}
			catch (Exception e)
			{
				ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
				throw;
			}
		}
	}
}
