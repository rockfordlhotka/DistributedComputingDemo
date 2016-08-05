using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	public interface ILogger
	{
		Task LogAsync(string message);
	}
}
