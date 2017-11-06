using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	public interface INotification
	{
		Task SendAsync(string recipient, string title, string message);
	}
}
