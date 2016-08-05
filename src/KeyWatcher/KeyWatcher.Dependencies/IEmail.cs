using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	public interface IEmail
	{
		Task SendAsync(string recipient, string title, string message);
	}
}
