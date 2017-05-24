using KeyWatcher.Messages;
using System.Web.Http;

namespace KeyWatcher.Azure.Controllers
{
	public sealed class KeyWatcherController
		: ApiController
	{
		public void Post([FromBody] UserKeysMessage value)
		{
			//  TODO: Need to call the IUser grain...this.holder.UsersActor.Tell(value);
		}
	}
}
