using Orleans;
using System.Threading.Tasks;

namespace KeyWatcher.OG.GrainInterfaces
{
	public interface IHello
		 : IGrainWithIntegerKey
	{
		Task<string> SayHello(string msg);
	}
}
