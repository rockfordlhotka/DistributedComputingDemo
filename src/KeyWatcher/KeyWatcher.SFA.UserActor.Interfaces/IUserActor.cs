using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using KeyWatcher.Messages;

namespace KeyWatcher.SFA.UserActor.Interfaces
{
	public interface IUserActor 
		: IActor
	{
		Task ProcessAsync(UserKeysMutableMessage message);
	}
}
