using KeyWatcher.Messages;
using Orleans;
using System.Threading.Tasks;

namespace KeyWatcher.Orleans.Contracts
{
	public interface IUserGrain
		: IGrainWithStringKey
	{
		Task Process(UserKeysMessage message);
	}
}
