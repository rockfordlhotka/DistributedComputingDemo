using KeyWatcher.Messages;
using Orleans;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Contracts
{
	public interface IUserGrain
		: IGrainWithStringKey
	{
		Task ProcessAsync(UserKeysMessage message);
	}
}
